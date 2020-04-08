.PHONY: init init-brew-deps

# To simplify things, we just try to init every framework even if
# they are not enabled.  However, we don't want to display hint messages
# about them not being enabled, so that's what this variable is used for

## Initialize dependencies for developing the project
init:
	@ echo "Installing prerequisites and setting up the environment ..."
	@ $(MAKE) _HIDDEN_IF_BOOTSTRAPPING=">/dev/null" \
		-- -init-frameworks
	@ echo "Done! 🍺"

## Install software for developing on macOS
init-brew-deps:
	@ if [ ! $(shell command -v "brew") ]; then \
		echo >&2 "$(_FATAL_ERROR) Please install Homebrew first (https://brew.sh)"; \
		exit 1; \
	fi
	$(Q) brew update $(OUTPUT_HIDDEN) && \
		 brew tap homebrew/bundle $(OUTPUT_HIDDEN) && \
		 brew bundle $(OUTPUT_HIDDEN) && \
		 direnv allow $(OUTPUT_HIDDEN)

-init-frameworks: | init-brew-deps ruby/init
