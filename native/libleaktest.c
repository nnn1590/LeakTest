#include <stdio.h>
#include "libleaktest.h"

void testCharPtr(ReturnCharPtrFunc func) {
	if (func == NULL) {
		fprintf(stderr, "Error: ReturnCharPtrFunc is null\n");
		fflush(stderr);
		return;
	}
	printf("ReturnCharPtrFunc: %s\n", func());
	fflush(stdout);
}

void testInt(ReturnIntFunc func) {
	if (func == NULL) {
		fprintf(stderr, "Error: ReturnIntFunc is null\n");
		fflush(stderr);
		return;
	}
	printf("ReturnIntFunc: %d\n", func());
	fflush(stdout);
}

void testIntPtr(ReturnIntPtrFunc func) {
	if (func == NULL) {
		fprintf(stderr, "Error: ReturnIntPtrFunc is null\n");
		fflush(stderr);
		return;
	}
	int* result = func();
	printf("ReturnIntFunc: ");
	if (result == NULL) printf("(null)\n");
	else printf("%d\n", *result);
	fflush(stdout);
}

TestStruct1 testStruct(TestStruct1 testStruct1) {
	printf("1: %s\n", testStruct1.str1);
	printf("2: %s\n", testStruct1.str2);
	printf("3: %s\n", testStruct1.str3);
	printf("4: %s\n", testStruct1.str4);
	printf("5: %s\n", testStruct1.str5);
	printf("6: %s\n", testStruct1.str6);
	printf("7: %s\n", testStruct1.str7);
	printf("8: %s\n", testStruct1.str8);
	printf("9: %s\n", testStruct1.str9);
	printf("10: %s\n", testStruct1.str10);
	fflush(stdout);
	return testStruct1;
}
