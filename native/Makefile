CC := gcc

libleaktest.so: libleaktest.c
	$(CC) -shared -fPIC -o $@ $^

.PHONY: clean
clean:
	$(RM) libleaktest.so
