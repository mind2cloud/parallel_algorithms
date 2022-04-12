#include <iostream>
#include <omp.h>
#include <math.h>
#include <time.h>
#include <vector>
using namespace std;

// ОБЫЧНОЕ - НЕПАРАЛЛЕЛЬНОЕ
double usualSummation(int N, int a) {
	double result(1.0);
	for (int i(2); i <= N; i += 1) {
		result += 1.0 / pow(i, a);
	}
	return result;
}

// ПАРАЛЛЕЛЬНОЕ
double parallelSummation(int N, int a, int M) {
	double result(1.0);
#pragma omp parallel for num_threads(M)
	for (int i(2); i <= N; i += 1) {
		result += 1.0 / pow(i, a);
	}
	return result;
}

int main() {
	int a, N, M;
	clock_t start(0), finish(0);
  // Данные по умолчанию
  a = 5; // степень
  N = 1000; // количество сумм
  M = 100; // количество потоков


  cout << "---------------------------------------------" << '\n';
	cout << "     Меняем степень a от 2 до 100" << '\n';
  cout << "---------------------------------------------" << '\n';
	
	for (int i(2); i <= 100; i += 1) {
    // время выполнения
    double processingTime = 0;
    //создаем массив из 100 элементов - количество измерений при каждой степени i
		vector<double> resultsOfSummation(100);

    for (int j(0); j < 100; j+=1){
      start = clock();
      resultsOfSummation[j] = parallelSummation(N, i, M);
      finish = clock();
      processingTime += (double)(finish - start);

    }
    cout << "(" << i << ";" << processingTime / 100.0 << ")";
	}


  cout << '\n' << '\n';
  cout << "---------------------------------------------" << '\n';
	cout << "    Меняем количество сумм N от 2 до 100" << '\n';
  cout << "---------------------------------------------" << '\n';
  cout << '\n';
	
	for (int i(2); i <= 100; i += 1) {
		start = clock();
    double processingTime = 0;
		vector<double> resultsOfSummation(100);

    for (int j(0); j < 100; j+=1){
      start = clock();
      resultsOfSummation[j] = parallelSummation(i, a, M);
      finish = clock();
      processingTime += (double)(finish - start);
    }
    cout << "(" << i << ";" << processingTime / 100.0 << ")";
	}
  
  cout << '\n' << '\n';
  cout << "---------------------------------------------" << '\n';
	cout << "   Меняем количество потоков M от 1 до 100" << '\n';
  cout << "---------------------------------------------" << '\n';
  cout << '\n';

	for (int i(1); i <= 100; i += 1) {
    double processingTime = 0;
		vector<double> resultsOfSummation(100);

    for (int j(0); j < 100; j += 1){
      start = clock();
      resultsOfSummation[j] = parallelSummation(N, a, i);
      finish = clock();
      processingTime += (double)(finish - start);
    }
    cout << "(" << i << ";" << processingTime / 100.0 << ")";
	}
  
	return 0;
}