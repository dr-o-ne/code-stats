FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

WORKDIR /app
COPY . ./
RUN dotnet publish -c Release -o out --no-self-contained

LABEL com.github.actions.name="CodeStats.io"
LABEL com.github.actions.description="CodeStats.io"
LABEL com.github.actions.icon="activity"
LABEL com.github.actions.color="orange"

FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "/CodeStats.GithubAction.dll" ]