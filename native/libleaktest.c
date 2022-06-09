#include <stdio.h>
#include "libleaktest.h"

void testCharPtr(ReturnCharPtrFunc func) {
	if (func == NULL) {
		fprintf(stderr, "Error: ReturnCharPtrFunc is null\n");
		return;
	}
	printf("ReturnCharPtrFunc: %s\n", func());
}

void testInt(ReturnIntFunc func) {
	if (func == NULL) {
		fprintf(stderr, "Error: ReturnIntFunc is null\n");
		return;
	}
	printf("ReturnIntFunc: %d\n", func());
}

void testIntPtr(ReturnIntPtrFunc func) {
	if (func == NULL) {
		fprintf(stderr, "Error: ReturnIntPtrFunc is null\n");
		return;
	}
	int* result = func();
	printf("ReturnIntFunc: ");
	if (result == NULL) printf("(null)\n");
	else printf("%d\n", *result);
}
