#include "iostream"
//#include <conio.h>
//#include "windows.h"
#include "omp.h"

void qSortParallel(float* a, const long n) {
    long i = 0, j = n;
    float mid = a[n / 2];

    do {
        while (a[i] < mid) i++;
        while (a[j] > mid) j--;

        if (i <= j) {
            std::swap(a[i], a[j]);
            i++; j--;
        }
    } while (i <= j);



#pragma omp parallel sections num_threads(4)
    {
#pragma omp section
        {
            if (j > 0) qSortParallel(a, j);
        }
#pragma omp section
        {
            if (n > i) qSortParallel(a + i, n - i);
        }
    }
}

void qSortUsual(float* a, const long n) {
    long i = 0, j = n;
    float mid = a[n / 2];

    do {
        while (a[i] < mid) i++;
        while (a[j] > mid) j--;

        if (i <= j) {
            std::swap(a[i], a[j]);
            i++; j--;
        }
    } while (i <= j);

    if (j > 0) qSortUsual(a, j);
    if (n > i) qSortUsual(a + i, n - i);
}

int main()
{
    if (1) {
        float aParallel[] = { 1, 4, 3, 7, 11, 2, 3, 5, 0, 10, 100, 500, 524, 26, 56, 78, 43, 57, 89, 20, 34, 44,23,87,66,55,55,43,646,30 };
        float bParallel[] = { 1.3, 0.3, 42.2, 23.6, 12.4 };

        clock_t start, finish;

        for (int i = 0; i < 30; i++) {
            std::cout << aParallel[i] << ", ";
        }

        std::cout << "\n" << '\n';

        for (int i = 0; i < 5; i++) {
            std::cout << bParallel[i] << ", ";
        }

        std::cout << '\n' << '\n';

        // ПАРАЛЛЕЛЬНОЕ

        start = clock();

        qSortParallel(aParallel, 29);
        qSortParallel(bParallel, 4);

        finish = clock();

        double processing_timeParallel = (double(finish-start));

        std::cout << "Parallel time: " << processing_timeParallel << '\n';

        // ОБЫЧНОЕ - НЕПАРАЛЛЕЛЬНОЕ

        float aUsual[] = { 1, 4, 3, 7, 11, 2, 3, 5, 0, 10, 100, 500, 524, 26, 56, 78, 43, 57, 89, 20, 34, 44,23,87,66,55,55,43,646,30 };
        float bUsual[] = { 1.3, 0.3, 42.2, 23.6, 12.4 };

        start = clock();

        //time_t start, end;
        //time (&start);

        qSortUsual(aUsual, 29);
        qSortUsual(bUsual, 4);

        //time (&end);


        finish = clock();
        double processing_timeUsual = (double(finish-start));

        std::cout << "Usual time: " << processing_timeUsual << '\n';


        //std::cout << "Time: " << difftime(end, start) << '\n';
    }

    //while (_getch() != 27);

    return 0;
}
