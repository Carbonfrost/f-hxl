.PHONY: dotnet/generate dotnet/test

## Generate generated code
dotnet/generate:
	srgen -c Carbonfrost.Commons.Hxl.Resources.SR \
		-r Carbonfrost.Commons.Hxl.Automation.SR \
		--resx \
		dotnet/src/Carbonfrost.Commons.Hxl/Automation/SR.properties
	srgen -c Hxlc.Resources.SR \
		-r Hxlc.Resources.SR \
		--resx \
		dotnet/src/hxlc/Automation/SR.properties
	/bin/sh -c "t4 -c Carbonfrost.Commons.Hxl.Compiler.ClassName dotnet/src/Carbonfrost.Commons.Hxl/Automation/TextTemplates/cs-TemplateFactorySkeleton.g.tt -o dotnet/src/Carbonfrost.Commons.Hxl/Automation/TextTemplates/cs-TemplateFactorySkeleton.g.cs"
	/bin/sh -c "t4 -c Carbonfrost.Commons.Hxl.Compiler.ClassName dotnet/src/Carbonfrost.Commons.Hxl/Automation/TextTemplates/cs-TemplateSkeleton.g.tt -o dotnet/src/Carbonfrost.Commons.Hxl/Automation/TextTemplates/cs-TemplateSkeleton.g.cs"

## Build the dotnet solution
dotnet/build:
	@ eval $(shell eng/build_env); \
		dotnet build --configuration $(CONFIGURATION) ./dotnet

## Execute dotnet unit tests
dotnet/test: dotnet/publish -dotnet/test

PUBLISH_DIR=dotnet/test/Carbonfrost.UnitTests.Hxl/bin/$(CONFIGURATION)/netcoreapp3.0/publish
NUGET_DIR=$(HOME)/.nuget/packages

-dotnet/test:
	fspec -i dotnet/test/Carbonfrost.UnitTests.Hxl/Content \
		-- \
		$(NUGET_DIR)/System.Memory/4.5.2/lib/netstandard2.0/System.Memory.dll \
		$(NUGET_DIR)/System.Text.Encoding.CodePages/4.5.1/lib/netstandard2.0/System.Text.Encoding.CodePages.dll \
		$(NUGET_DIR)/System.Runtime.CompilerServices.Unsafe/4.5.2/lib/netstandard2.0/System.Runtime.CompilerServices.Unsafe.dll \
		$(NUGET_DIR)/System.Threading.Tasks.Extensions/4.5.2/lib/netstandard2.0/System.Threading.Tasks.Extensions.dll \
		$(PUBLISH_DIR)/Carbonfrost.UnitTests.Hxl.dll

include eng/.mk/*.mk
