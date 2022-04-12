#include <iostream>
#include <vector>
#include <chrono>
#include <omp.h>

using namespace std;

double usualScalarProduct(vector<double> a, vector<double> b, int N)
{
    double res = 0;
    for (int i = 0; i < N; i++)
    {
        res += a[i] * b[i];
    }
    return res;
}

double parallelScalarProduct(vector<double> a, vector<double> b, int N)
{
    double res = 0;
    
#pragma omp parallel for reduction(+:res)
    for (int i = 0; i < N; i++)
    {
#pragma omp atomic
        res += a[i] * b[i];
    }
    
    return res;
}

int main()
{
    int N = 10000000;
    vector<double> v1(N);
    vector<double> v2(N);
    
    int A = 1;
    int B = 1000;
    
    for (int i = 0; i < N; i++)
    {
        v1[i] = (rand() % 1000 - 500) / 100;
        v2[i] = (rand() % 1000 - 500) / 100;
    }
    
    chrono::time_point<chrono::system_clock> start, end;
    int resultTime;
    
    start = chrono::system_clock::now();
    cout << "Последовательный результат скалярного произведения: " << usualScalarProduct(v1, v2, N) << '\n';
    end = chrono::system_clock::now();
    
    resultTime = chrono::duration_cast<chrono::milliseconds>(end - start).count();
    cout << "Последовательное время: " << resultTime << '\n' << '\n';
    
    start = chrono::system_clock::now();
    cout << "Параллельный результат скалярного произведения: " << parallelScalarProduct(v1, v2, N) << '\n';
    end = chrono::system_clock::now();
    
    resultTime = chrono::duration_cast<chrono::milliseconds>(end - start).count();
    cout << "Параллельное время: " << resultTime << '\n';
    
    return 0;
}
