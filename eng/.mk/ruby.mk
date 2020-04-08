ENG_AUTODETECT_USING_RUBY = $(shell [ ! -f .ruby-version ] ; echo $$?)
ENG_USING_RUBY ?= $(ENG_AUTODETECT_USING_RUBY)

ENG_LATEST_RUBY_VERSION = 2.6.0

.PHONY: \
	-hint-unsupported-ruby \
	-ruby/init \
	-use/ruby \
	-use/ruby-Gemfile \
	-use/ruby-version \
	ruby/init \
	use/ruby \

use/ruby: | -use/ruby-version -ruby/init -use/ruby-Gemfile

# Enable the tasks if we are using ruby
ifeq (1, $(ENG_USING_RUBY))

## Install Ruby and project dependencies
ruby/init: -ruby/init
else
ruby/init: -hint-unsupported-ruby
endif

-ruby/init: -check-command-rbenv
	@    echo "Installing Ruby and Ruby dependencies..."
	$(Q) rbenv install -s $(OUTPUT_HIDDEN)
	$(Q) gem install bundler  $(OUTPUT_HIDDEN)
	$(Q) [ -f Gemfile ] && bundle install  $(OUTPUT_HIDDEN)

-use/ruby-Gemfile: -check-command-Gemfile
	$(Q) [ -f Gemfile ] || bundle init

-use/ruby-version:
	@    echo "Adding support for Ruby to this project... "
	$(Q) [ -f .ruby-version ] || echo $(ENG_LATEST_RUBY_VERSION) > .ruby-version

-hint-unsupported-ruby:
	@ echo $(_HIDDEN_IF_BOOTSTRAPPING) "$(_WARNING) Nothing to do" \
		"because $(_MAGENTA)Ruby$(_RESET) is not enabled (Investigate $(_CYAN)\`make use/ruby\`$(_RESET))"
