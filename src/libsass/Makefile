OS ?= $(shell uname -s)
CC ?= cc
CXX ?= c++
RM ?= rm -rf
MKDIR_P ?= mkdir -p
CFLAGS ?= -Wall
CXXFLAGS ?= -Wall
LDFLAGS ?= -Wall
ifndef COVERAGE
	CFLAGS += -O2
	CXXFLAGS += -O2
	LDFLAGS += -O2
else
	CFLAGS += -O1 -fno-omit-frame-pointer
	CXXFLAGS += -O1 -fno-omit-frame-pointer
	LDFLAGS += -O1 -fno-omit-frame-pointer
endif
LDFLAGS += -Wl,-undefined,error

CXXFLAGS += -std=c++11
LDFLAGS += -std=c++11

CFLAGS += -I include
CXXFLAGS += -I include

CFLAGS += $(EXTRA_CFLAGS)
CXXFLAGS += $(EXTRA_CXXFLAGS)
LDFLAGS += $(EXTRA_LDFLAGS)

LDLIBS = -lm

ifeq ($(OS),Darwin)
	CFLAGS += -stdlib=libc++
	CXXFLAGS += -stdlib=libc++
	LDFLAGS += -stdlib=libc++

	PLATFORM = OSX
	ASSEMBLY_TYPE_ARG = -dynamiclib
	ASSEMBLY_EXT = .dylib
else
	PLATFORM = Linux
	ASSEMBLY_TYPE_ARG = -shared
	ASSEMBLY_EXT = .so
endif

ASSEMBLY_PATH = $(BIN_DIR)/libsass$(ASSEMBLY_EXT)

ifneq (FreeBSD,$(OS))
	ifneq (OpenBSD,$(OS))
		LDFLAGS += -ldl
		LDLIBS += -ldl
	endif
endif

ifeq ($(DEBUG),1)
	TARGET = debug
	CONFIGURATION = Debug
else
	TARGET = build
	CONFIGURATION = Release
endif

BIN_DIR = bin/$(CONFIGURATION)/$(PLATFORM)
OBJECT_DIR = obj/$(CONFIGURATION)/$(PLATFORM)

CFLAGS += -fPIC
CXXFLAGS += -fPIC
LDFLAGS += -fPIC

include Makefile.conf

OBJECT_SUBDIRS = $(addprefix $(OBJECT_DIR)/,$(SOURCE_SUBDIRS))
OBJECTS = $(addprefix $(OBJECT_DIR)/,$(SOURCES:.cpp=.o))
COBJECTS = $(addprefix $(OBJECT_DIR)/,$(CSOURCES:.c=.o))

DEBUG_LVL ?= NONE

CLEANUPS ?=
CLEANUPS += $(COBJECTS)
CLEANUPS += $(OBJECTS)

all: $(TARGET)

debug: LDFLAGS := -g $(filter-out -O2,$(LDFLAGS))
debug: CFLAGS := -g -DDEBUG -DDEBUG_LVL="$(DEBUG_LVL)" $(filter-out -O2,$(CFLAGS))
debug: CXXFLAGS := -g -DDEBUG -DDEBUG_LVL="$(DEBUG_LVL)" $(filter-out -O2,$(CXXFLAGS))
debug: build

build: $(ASSEMBLY_PATH)

$(ASSEMBLY_PATH): make-output-dirs $(COBJECTS) $(OBJECTS)
	$(CXX) $(ASSEMBLY_TYPE_ARG) $(LDFLAGS) -o $@ $(COBJECTS) $(OBJECTS) $(LDLIBS)

make-output-dirs:
	$(MKDIR_P) $(BIN_DIR) $(OBJECT_DIR) $(OBJECT_SUBDIRS)

$(OBJECT_DIR)/%.o: %.c
	$(CC) $(CFLAGS) -c -o $@ $<

$(OBJECT_DIR)/%.o: %.cpp
	$(CXX) $(CXXFLAGS) -c -o $@ $<

clean: clean-objects
	$(RM) $(BIN_DIR)/*.so $(BIN_DIR)/*.dylib

clean-objects:
	$(RM) $(CLEANUPS)

.PHONY: all clean clean-objects debug make-output-dirs