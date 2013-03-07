#ifndef _JV_POOL_H_INCLUDED_
#define _JV_POOL_H_INCLUDED_

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
//#include <jv_string.h>
#define jv_memzero(buf, n)          (void *) memset(buf, 0, n)

#define JV_OK       0
#define JV_ERROR    -1

typedef unsigned char   u_char;
typedef int             jv_int_t;
typedef unsigned int    jv_uint_t;

typedef struct  jv_pool_s   jv_pool_t;
typedef struct  jv_block_s  jv_block_t;
typedef struct  jv_lump_s   jv_lump_t;

#define JV_ALIGNMENT   sizeof(unsigned long)    /* platform word */

#define JV_BLOCK_MAX_SIZE   4096//getpagesize()

#define JV_BLOCK_MIN_SIZE   128

#define JV_ALLOC_MAX_SIZE   (JV_BLOCK_MAX_SIZE - sizeof(jv_block_t) - 2 * sizeof(jv_lump_t))

#define jv_align(d, a)     (((d) + (a - 1)) & ~(a - 1))


/*
#define jv_align_ptr(p, a)                                                   \
    (u_char *) (((jv_uint_it) (p) + ((jv_uint_t) a - 1)) & ~((jv_uint_t) a - 1))

*/

/*
    jv_block_t ���õ����ѭ��������β��㱣����jv_pool_t��
    �Ա���������jv_block_tʱ������β������
*/
struct jv_block_s { /* 4 */
    jv_block_t  *next;
};

/*
    jv_lump_t ����˫��ѭ��������Ҫ��Ϊ�˿��Ը�Ч���շ�����ڴ�
    ��ʵ��jv_lump_t���Խ�Ϊ����ѭ�����������ڻ��շ�����ڴ�ʱ
    ��Ҫ�ȱ��������ڴ��
*/
struct jv_lump_s { /* 16 */
    size_t      size;
    jv_lump_t   *prev;
    jv_lump_t   *next;
    unsigned    used:1;
};

struct jv_pool_s {  /* 20 */
    size_t          max;        /* block max size */
    jv_block_t      *first;
    jv_block_t      *last;
    jv_lump_t       *lump;
    jv_lump_t       *pos;       /* current idle lump's position */
};

jv_pool_t *jv_pool_create (size_t size);
void *jv_pool_alloc (jv_pool_t *pool, size_t size);
void *jv_pool_calloc (jv_pool_t *pool, size_t size);
jv_int_t jv_pool_free (jv_pool_t *pool, void *p);
jv_int_t jv_pool_destroy (jv_pool_t *pool);

#endif /* _JV_POOL_H_INCLUDED_ */