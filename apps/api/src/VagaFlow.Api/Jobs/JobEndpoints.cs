using Microsoft.EntityFrameworkCore;
using Npgsql;
using VagaFlow.Api.Persistence;

namespace VagaFlow.Api.Jobs;

public static class JobEndpoints
{
    public static IEndpointRouteBuilder MapJobEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/jobs").WithTags("Jobs");

        group.MapPost("/manual", CreateManualJob);
        group.MapGet("", GetJobs);
        group.MapGet("/{id:guid}", GetJob);

        return app;
    }

    private static async Task<IResult> CreateManualJob(
        ManualJobRequest request,
        VagaFlowDbContext db,
        CancellationToken cancellationToken)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        _ = JobUrl.TryNormalize(request.Url, out var canonicalUrl);

        if (await db.Jobs.AnyAsync(job => job.CanonicalUrl == canonicalUrl, cancellationToken))
        {
            return Results.Conflict(new { message = "Essa vaga já está registrada no Radar." });
        }

        var job = new JobListing
        {
            Id = Guid.NewGuid(),
            Company = request.Company!.Trim(),
            Title = request.Title!.Trim(),
            OriginalUrl = request.Url!.Trim(),
            CanonicalUrl = canonicalUrl!,
            SourcePortal = string.IsNullOrWhiteSpace(request.SourcePortal)
                ? new Uri(request.Url.Trim()).Host
                : request.SourcePortal.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Area = request.Area,
            WorkArrangement = request.WorkArrangement,
            City = string.IsNullOrWhiteSpace(request.City) ? null : request.City.Trim(),
            State = string.IsNullOrWhiteSpace(request.State) ? null : request.State.Trim().ToUpperInvariant(),
            PublishedAtUtc = request.PublicationDateVerified ? request.PublishedAtUtc?.UtcDateTime : null,
            FirstSeenAtUtc = DateTime.UtcNow,
            VerificationState = JobVerificationState.PendingReview
        };

        db.Jobs.Add(job);
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException error) when
            (error.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            return Results.Conflict(new { message = "Essa vaga já está registrada no Radar." });
        }

        return Results.Created($"/api/v1/jobs/{job.Id}", JobResponse.FromEntity(job));
    }

    private static async Task<IResult> GetJobs(
        VagaFlowDbContext db,
        string? area,
        string? workArrangement,
        string? city,
        string? state,
        DateOnly? publishedSince,
        CancellationToken cancellationToken)
    {
        IQueryable<JobListing> query = db.Jobs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(area))
        {
            if (!Enum.TryParse<JobArea>(area, true, out var parsedArea) ||
                !Enum.IsDefined(parsedArea))
            {
                return Results.BadRequest(new { message = "Área de vaga inválida." });
            }
            query = query.Where(job => job.Area == parsedArea);
        }

        if (!string.IsNullOrWhiteSpace(workArrangement))
        {
            if (!Enum.TryParse<WorkArrangement>(workArrangement, true, out var parsedMode) ||
                !Enum.IsDefined(parsedMode))
            {
                return Results.BadRequest(new { message = "Modalidade inválida." });
            }
            query = query.Where(job => job.WorkArrangement == parsedMode);
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(job => job.City != null && job.City.ToLower() == city.Trim().ToLower());
        }

        if (!string.IsNullOrWhiteSpace(state))
        {
            query = query.Where(job => job.State == state.Trim().ToUpperInvariant());
        }

        if (publishedSince.HasValue)
        {
            var cutoff = publishedSince.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            query = query.Where(job => job.PublishedAtUtc != null && job.PublishedAtUtc >= cutoff);
        }

        var results = await query.OrderByDescending(job => job.FirstSeenAtUtc)
            .ThenBy(job => job.Company)
            .Take(100)
            .ToListAsync(cancellationToken);

        return Results.Ok(results.Select(JobResponse.FromEntity));
    }

    private static async Task<IResult> GetJob(
        Guid id,
        VagaFlowDbContext db,
        CancellationToken cancellationToken)
    {
        var job = await db.Jobs.AsNoTracking()
            .SingleOrDefaultAsync(job => job.Id == id, cancellationToken);

        return job is null ? Results.NotFound() : Results.Ok(JobResponse.FromEntity(job));
    }

    private static Dictionary<string, string[]> Validate(ManualJobRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Company) || request.Company.Length > 160)
            errors["company"] = ["Informe o nome da empresa (até 160 caracteres)."];
        if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 180)
            errors["title"] = ["Informe o cargo (até 180 caracteres)."];
        if (!JobUrl.TryNormalize(request.Url, out _))
            errors["url"] = ["Informe um link HTTPS externo e válido para a vaga."];
        if (request.SourcePortal?.Length > 120)
            errors["sourcePortal"] = ["Fonte excede 120 caracteres."];
        if (request.Description?.Length > 20000)
            errors["description"] = ["A descrição deve ter até 20.000 caracteres."];
        if (request.City?.Length > 120)
            errors["city"] = ["Cidade excede 120 caracteres."];
        if (request.State is not null &&
            (request.State.Trim().Length != 2 || !request.State.Trim().All(char.IsLetter)))
            errors["state"] = ["Estado deve ser uma sigla com duas letras."];
        if (!Enum.IsDefined(request.Area))
            errors["area"] = ["Área inválida."];
        if (!Enum.IsDefined(request.WorkArrangement))
            errors["workArrangement"] = ["Modalidade inválida."];
        if (request.PublishedAtUtc is not null && !request.PublicationDateVerified)
            errors["publishedAtUtc"] = ["Confirme a data na fonte antes de registrá-la como publicada."];
        if (request.PublicationDateVerified && request.PublishedAtUtc is null)
            errors["publishedAtUtc"] = ["Data publicada deve ser informada quando confirmada."];

        return errors;
    }
}
