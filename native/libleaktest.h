#ifdef __cplusplus
extern "C" {
#endif

typedef int   (*ReturnIntFunc)(void);
typedef int*  (*ReturnIntPtrFunc)(void);
typedef char* (*ReturnCharPtrFunc)(void);

typedef struct {
	char* str1;
	char* str2;
	char* str3;
	char* str4;
	char* str5;
	char* str6;
	char* str7;
	char* str8;
	char* str9;
	char* str10;
} TestStruct1;

TestStruct1 testStruct(TestStruct1 testStruct1);

#ifdef __cplusplus
}
#endif
