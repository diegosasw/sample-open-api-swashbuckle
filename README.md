# sample-open-api
Open Api definitions available in GitHub pages at https://diegosasw.github.io/sample-open-api/

## Exporting OpenAPI Documents
The OpenApi documents can be 
1. manually generated
2. manually downloaded
3. automatically exported

Since the sample web applications are created using AspNetCore and Swashbuckle, there is an available dotnet tool
to export the OpenApi document which could be triggered during CI/CD pipeline and copied into the `/docs` folder
so that the documents are served by http.

## Local Development
This solution follows one possible approach to generate and publish OpenApi documents for testing purposes.

It uses `Swashbuckle.AspNetCore.Cli` dotnet tool installed locally.

When cloning the repository, run the following to restore nuget dependencies
```
dotnet restore
```

and then run the following to restore dotnet tools
```
dotnet tool restore
```

NOTE: This solution includes a `dotnet-tools.json` stating the dotnet tools to restore because a manifest was created
with `dotnet new tool-manifest` and a local dotnet tool was added to the manifest with `dotnet tool install Swashbuckle.AspNetCore.Cli`.

Compile the solution
```
dotnet build -c Release
```

Now in the bin/Release folder for each project there will be a `Swashbuckle.AspNetCore.Swagger.dll` 
with a version matching the CLI.

Create a folder where to place all the exported OpenApi documents
```
mkdir temp
```

Now everything is ready to export the OpenApi document with the command
```
dotnet swagger tofile [options] [startupassembly] [swaggerdoc]
```

The arguments are described below: 
- `options` can be used to configure a host to include, a basepath, the format (e.g: yaml) or the output. For example, 
  the`--output` followed by a relative path where to export the OpenApi document, otherwise it'll be displayed on stdout.
- `startupassembly` is the path to the dll which contains the Startup
- `swaggerdoc` is the swagger doc name, by default `v1`, which can be configured in the options of the `AddSwaggerGen` method.

Run the following:
```
dotnet swagger tofile --output temp/web-api-one.json src/WebApiOne/bin/Release/net8.0/WebApiOne.dll web-api-one
```
and
```
dotnet swagger tofile --output temp/web-api-two.json src/WebApiTwo/bin/Release/net8.0/WebApiTwo.dll web-api-two
```

The two OpenApi documents will be placed in the `temp` folder ready to be moved to the `docs/`

```
mv temp/* docs/
```

The `docs` folder now has the two OpenApi documents. This folder can released in a static http server or similar so that
there are links to the OpenAPI documents that other tools or applications can use.

## Tokens used 
There is a need to create a Personal Access Token with `repo` and `workflow` scopes (i.e: `PAT_TRIGGER_REPO`) to trigger repository dispatch with custom event.

## Common Issues

### Error Generating OpenApi Document
Make sure the `Swashbuckle.AspNetCore` dependency version matches the CLI.
If your CLI has a different version, the export might fail, that's why using versioned `dotnet-tools.json` manifest
is recommended to avoid mismatches.

Check dotnet tool version with:
```
dotnet tool list
```