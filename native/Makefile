CC := gcc

.PHONY: all
all: libleaktest.so

libleaktest.so: libleaktest.c libleaktest.h Makefile
	$(CC) -Wall -Wextra -shared -fPIC -o $@ libleaktest.c

.PHONY: clean
clean:
	$(RM) libleaktest.so
