#
# A standard system for building a dotnet assembly.  This should be included from the
# main Makefile
#
# Structure of the repository should be:
#
#    dotnet  (default root directory, configured with $ENG_DOTNET_DIR)
#    ├── Directory.Build.props   -- (Optional) Shared build attributes controlling version, etc.
#    ├── solution.sln
#    ├── src
#    │   └── Project1
#    │       ├── Automation  -- (Optional) String resources, text templates, etc.
#    │       │   ├── SR.properties
#    │       │   └── TextTemplates
#    │       ├── Project1.csproj
#    │       └── (source files)
#    └── test
#        └── Project1.UnitTests
#            ├── Project1.UnitTests.csproj
#            ├── Content    -- (Optional) Fixture files
#            │   └── ...
#            └── (source files)
#

.PHONY: \
	-dotnet/build \
	-dotnet/pack \
	-dotnet/publish \
	-dotnet/push \
	dotnet/build \
	dotnet/clean \
	dotnet/configure \
	dotnet/pack \
	dotnet/publish \
	dotnet/push \
	dotnet/restore \
	dotnet/test \

## Add support for .NET to the project
use/dotnet: | -dotnet/init -dotnet/solution

# Enable the tasks if we are using dotnet
ifeq (1, $(ENG_USING_DOTNET))

## Install .NET  and project dependencies
dotnet/init: -dotnet/init dotnet/restore
else
dotnet/init: -hint-unsupported-dotnet
endif

## Set up dotnet configuration for NuGet
dotnet/configure: -requirements-dotnet -check-env-NUGET_SOURCE_URL -check-env-NUGET_PASSWORD -check-env-NUGET_USER_NAME -check-env-NUGET_CONFIG_FILE
	$(Q) test -e $(NUGET_CONFIG_FILE) || echo "<configuration />" > $(NUGET_CONFIG_FILE)
	$(Q) nuget sources add -Name "Carbonfrost" \
		-Source $(NUGET_SOURCE_URL) \
		-Password $(NUGET_PASSWORD) \
		-Username $(NUGET_USER_NAME) \
		-StorePasswordInClearText \
		-ConfigFile $(NUGET_CONFIG_FILE)

## Restore package dependencies
dotnet/restore: -requirements-dotnet
	$(Q) dotnet restore $(ENG_DOTNET_DIR)
	$(Q) dotnet tool restore

## Build the dotnet solution
dotnet/build: dotnet/restore -dotnet/build

## Pack the dotnet build into a NuGet package
dotnet/pack: dotnet/build -dotnet/pack

## Push the dotnet build into package repository
dotnet/push: dotnet/pack -dotnet/push

## Executes dotnet publish
dotnet/publish: dotnet/build -dotnet/publish

## Executes dotnet clean
dotnet/clean:
	$(Q) rm -rdf $(ENG_DOTNET_DIR)/{src,test}/*/{bin,obj}/*

-dotnet/init:
	$(Q) brew cask install dotnet-sdk $(OUTPUT_HIDDEN)

-dotnet/solution:
	$(Q) mkdir -p $(ENG_DOTNET_DIR)/{src,test}
	$(Q) dotnet new sln -o $(ENG_DOTNET_DIR) -n $(shell basename $$(pwd))

-dotnet/build: -requirements-dotnet -check-env-CONFIGURATION
	$(Q) eval $(shell eng/build_env); \
		dotnet build --configuration $(CONFIGURATION) --no-restore $(ENG_DOTNET_DIR)

-dotnet/pack: -requirements-dotnet -check-env-CONFIGURATION
	$(Q) eval $(shell eng/build_env); \
		dotnet pack --configuration $(CONFIGURATION) --no-build $(ENG_DOTNET_DIR)

-dotnet/publish: -requirements-dotnet -check-env-CONFIGURATION
	$(Q) eval $(shell eng/build_env); \
		dotnet publish --configuration $(CONFIGURATION) --no-build $(ENG_DOTNET_DIR)

# Nuget CLI doesn't work with GitHub package registry for some reason, so we're using a curl directly
-dotnet/push: -requirements-dotnet -check-env-NUGET_PASSWORD -check-env-NUGET_USER_NAME -check-env-NUGET_UPLOAD_URL
	$(Q) for f in dotnet/src/*/bin/Release/*.{nupkg,snupkg}; do \
		curl -X PUT -u "$(NUGET_USER_NAME):$(NUGET_PASSWORD)" -F package=@$$f $(NUGET_UPLOAD_URL); \
	done

-requirements-dotnet: -check-command-dotnet

-hint-unsupported-dotnet:
	@ echo $(_HIDDEN_IF_BOOTSTRAPPING) "$(_WARNING) Nothing to do" \
		"because $(_MAGENTA).NET$(_RESET) is not enabled (Investigate $(_CYAN)\`make use/dotnet\`$(_RESET))"
