

# Prefix to install
PREFIX ?= /usr/local

# ------- .NET settings
#

# Whether we are meant to use .NET
ENG_USING_DOTNET = 1

# Directory to use as root of a dotnet project
ENG_DOTNET_DIR ?= dotnet/

# The location of the NuGet configuration file
NUGET_CONFIG_FILE ?= ./nuget.config

# The configuration to build (probably "Debug" or "Release")
CONFIGURATION ?= Release

# The framework to publish
FRAMEWORK ?= netcoreapp3.0

# ------- Ruby settings
#

# Whether we are meant to use Ruby.  (See ruby.mk for autodetection)
ENG_USING_RUBY ?= $(ENG_AUTODETECT_USING_RUBY)

# Latest version of Ruby supported
ENG_LATEST_RUBY_VERSION = 2.6.0

# Whether to turn on command silencing depends on whether VERBOSE was set
ifneq (, $(VERBOSE))
Q =
OUTPUT_HIDDEN =
else
Q = @
OUTPUT_HIDDEN = >/dev/null
endif

# These variables are meant to be used internally

# Terminal output formatting
_RESET = \x1b[39m
_YELLOW = \x1b[33m
_GREEN = \x1b[32m
_RED = \x1b[31m
_MAGENTA = \x1b[35m
_UNDERLINE = \x1b[4m

_CYAN = \x1b[36m

_FATAL_ERROR = $(_RED)fatal: $(_RESET)
_WARNING = $(_YELLOW)warning: $(_RESET)
