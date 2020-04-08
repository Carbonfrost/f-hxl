# The configuration to build (probably "Debug" or "Release")
CONFIGURATION ?= Release

# The framework to publish
FRAMEWORK ?= netcoreapp3.0

# The location of the NuGet configuration file
NUGET_CONFIG_FILE ?= ./nuget.config

# Prefix to install
PREFIX ?= /usr/local

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
_RED = \x1b[31m
_MAGENTA = \x1b[35m
_CYAN = \x1b[36m

_FATAL_ERROR = $(_RED)fatal: $(_RESET)
_WARNING = $(_YELLOW)warning: $(_RESET)