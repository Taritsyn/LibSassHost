OS       ?= $(shell uname -s)
CC       ?= gcc
CXX      ?= g++
RM       ?= rm -f
MKDIR    ?= mkdir
RMDIR    ?= rmdir
# Solaris/Illumos flavors
# ginstall from coreutils
ifeq ($(OS),SunOS)
	INSTALL  ?= ginstall
endif
INSTALL  ?= install
CFLAGS   ?= -Wall
CXXFLAGS ?= -Wall
LDFLAGS  ?= -Wall
ifneq "$(COVERAGE)" "yes"
  CFLAGS   += -O2
  CXXFLAGS += -O2
  LDFLAGS  += -O2
endif
LDFLAGS  += -Wl,-undefined,error

UNAME := $(shell uname -s)

ifeq ($(SASS_LIBSASS_PATH),)
	SASS_LIBSASS_PATH = $(abspath $(CURDIR))
endif

CXXFLAGS += -std=c++0x
LDFLAGS  += -std=c++0x

CFLAGS   += -I $(SASS_LIBSASS_PATH)/include
CXXFLAGS += -I $(SASS_LIBSASS_PATH)/include

ifneq ($(EXTRA_CFLAGS),)
	CFLAGS   += $(EXTRA_CFLAGS)
endif
ifneq ($(EXTRA_CXXFLAGS),)
	CXXFLAGS += $(EXTRA_CXXFLAGS)
endif
ifneq ($(EXTRA_LDFLAGS),)
	LDFLAGS  += $(EXTRA_LDFLAGS)
endif

LDLIBS = -lm

ifeq ($(UNAME),Darwin)
	CFLAGS += -stdlib=libc++
	CXXFLAGS += -stdlib=libc++
	LDFLAGS += -stdlib=libc++
endif

ifneq (FreeBSD,$(UNAME))
	ifneq (OpenBSD,$(UNAME))
		LDFLAGS += -ldl
		LDLIBS += -ldl
	endif
endif

ifneq ($(BUILD),dynamic)
	BUILD := shared
endif
ifeq ($(DEBUG),1)
	BUILD := debug-$(BUILD)
endif

SHARED_LIB_EXT = .so
DYNAMIC_LIB_EXT = .dylib

SHARED_LIB_REL_PATH = lib/libsass$(SHARED_LIB_EXT)
DYNAMIC_LIB_REL_PATH = lib/libsass$(DYNAMIC_LIB_EXT)

SHARED_LIB_ABS_PATH = $(SASS_LIBSASS_PATH)/$(SHARED_LIB_REL_PATH)
DYNAMIC_LIB_ABS_PATH = $(SASS_LIBSASS_PATH)/$(DYNAMIC_LIB_REL_PATH)

CFLAGS   += -fPIC
CXXFLAGS += -fPIC
LDFLAGS  += -fPIC

include Makefile.conf

OBJECTS = $(SOURCES:.cpp=.o)
COBJECTS = $(CSOURCES:.c=.o)

DEBUG_LVL ?= NONE

CLEANUPS ?=
CLEANUPS += $(COBJECTS)
CLEANUPS += $(OBJECTS)

all: $(BUILD)

debug: $(BUILD)

debug-shared: LDFLAGS := -g $(filter-out -O2,$(LDFLAGS))
debug-shared: CFLAGS := -g -DDEBUG -DDEBUG_LVL="$(DEBUG_LVL)" $(filter-out -O2,$(CFLAGS))
debug-shared: CXXFLAGS := -g -DDEBUG -DDEBUG_LVL="$(DEBUG_LVL)" $(filter-out -O2,$(CXXFLAGS))
debug-shared: shared

debug-dynamic: LDFLAGS := -g $(filter-out -O2,$(LDFLAGS))
debug-dynamic: CFLAGS := -g -DDEBUG -DDEBUG_LVL="$(DEBUG_LVL)" $(filter-out -O2,$(CFLAGS))
debug-dynamic: CXXFLAGS := -g -DDEBUG -DDEBUG_LVL="$(DEBUG_LVL)" $(filter-out -O2,$(CXXFLAGS))
debug-dynamic: dynamic

lib:
	$(MKDIR) lib

$(SHARED_LIB_REL_PATH): lib $(COBJECTS) $(OBJECTS)
	$(CXX) -shared $(LDFLAGS) -o $@ $(COBJECTS) $(OBJECTS) $(LDLIBS)

$(DYNAMIC_LIB_REL_PATH): lib $(COBJECTS) $(OBJECTS)
	$(CXX) -dynamiclib -o $@ $(COBJECTS) $(OBJECTS)

%.o: %.c
	$(CC) $(CFLAGS) -c -o $@ $<

%.o: %.cpp
	$(CXX) $(CXXFLAGS) -c -o $@ $<

install: install-$(BUILD)

shared: $(SHARED_LIB_REL_PATH)
dynamic: $(DYNAMIC_LIB_REL_PATH)

$(DESTDIR)$(PREFIX):
	$(MKDIR) $(DESTDIR)$(PREFIX)

$(DESTDIR)$(PREFIX)/lib: $(DESTDIR)$(PREFIX)
	$(MKDIR) $(DESTDIR)$(PREFIX)/lib

$(DESTDIR)$(PREFIX)/include: $(DESTDIR)$(PREFIX)
	$(MKDIR) $(DESTDIR)$(PREFIX)/include

$(DESTDIR)$(PREFIX)/include/sass: $(DESTDIR)$(PREFIX)/include
	$(MKDIR) $(DESTDIR)$(PREFIX)/include/sass

$(DESTDIR)$(PREFIX)/include/%.h: include/%.h \
                                 $(DESTDIR)$(PREFIX)/include \
                                 $(DESTDIR)$(PREFIX)/include/sass
	$(INSTALL) -v -m0644 "$<" "$@"

install-headers: $(DESTDIR)$(PREFIX)/include/sass.h \
                 $(DESTDIR)$(PREFIX)/include/sass2scss.h \
                 $(DESTDIR)$(PREFIX)/include/sass/base.h \
                 $(DESTDIR)$(PREFIX)/include/sass/version.h \
                 $(DESTDIR)$(PREFIX)/include/sass/values.h \
                 $(DESTDIR)$(PREFIX)/include/sass/context.h \
                 $(DESTDIR)$(PREFIX)/include/sass/functions.h \
                 $(DESTDIR)$(PREFIX)/include/file_manager.h #LSH+

$(DESTDIR)$(PREFIX)/lib/%$(SHARED_LIB_EXT): lib/%$(SHARED_LIB_EXT) \
                             $(DESTDIR)$(PREFIX)/lib
	@$(INSTALL) -v -m0755 "$<" "$@"

$(DESTDIR)$(PREFIX)/lib/%$(DYNAMIC_LIB_EXT): lib/%$(DYNAMIC_LIB_EXT) \
                             $(DESTDIR)$(PREFIX)/lib
	@$(INSTALL) -v -m0755 "$<" "$@"

install-shared: $(DESTDIR)$(PREFIX)/$(SHARED_LIB_REL_PATH) \
                install-headers

install-dynamic: $(DESTDIR)$(PREFIX)/$(DYNAMIC_LIB_REL_PATH) \
                install-headers

clean-objects: lib
	-$(RM) lib/*$(SHARED_LIB_EXT) lib/*$(DYNAMIC_LIB_EXT) lib/*.la
	-$(RMDIR) lib
clean: clean-objects
	$(RM) $(CLEANUPS)

lib-file: lib-file-$(BUILD)
lib-opts: lib-opts-$(BUILD)

lib-file-shared:
	@echo $(SHARED_LIB_ABS_PATH)
lib-file-dynamic:
	@echo $(DYNAMIC_LIB_ABS_PATH)
lib-opts-shared:
	@echo -L"$(SASS_LIBSASS_PATH)/lib -lsass"
lib-opts-dynamic:
	@echo -L"$(SASS_LIBSASS_PATH)/lib -lsass"

.PHONY: all shared dynamic \
        install-headers \
        clean clean-objects \
        debug debug-shared debug-dynamic \
        install install-shared install-dynamic \
        lib-opts lib-opts-shared lib-opts-dynamic \
        lib-file lib-file-shared lib-file-dynamic
.DELETE_ON_ERROR:
