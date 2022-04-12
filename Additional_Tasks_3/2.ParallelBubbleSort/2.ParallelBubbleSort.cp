#include <omp.h>
#include <iostream>
#include <time.h>
//#include <vector>
using namespace std;

// ОБЫЧНОЕ - НЕПАРАЛЛЕЛЬНОЕ
double* usualBubbleSort(double* sortedArray) {
	int size = 33;
  double temporary;

	for (int i(0); i < (size - 1); i += 1){
		for (int j(0); j < (size - i - 1); j += 1){
			if (sortedArray[j] > sortedArray[j + 1]){
				temporary = sortedArray[j];
				sortedArray[j] = sortedArray[j + 1];
				sortedArray[j + 1] = temporary;
			}
		}
	}
	return sortedArray;
}

// ПАРАЛЛЕЛЬНОЕ
double* parallelBubbleSort(double* sortedArray) {
  int size = 33;
  int upperElement;
  double temporary;
  
	if (size % 2 == 0) {
		upperElement = (size / 2)-1;
	} else {
    upperElement = size / 2;
  }

	for (int i = 0; i < size; i++) {
		
		if (i % 2 == 0) {
      #pragma omp parallel for
			for (int j(0); j < (size / 2); j += 1) {
				if (sortedArray[2 * j] > sortedArray[2 * j + 1]) {
				    temporary = sortedArray[2 * j];
					sortedArray[2 * j] = sortedArray[2 * j + 1];
					sortedArray[2 * j + 1] = temporary;
				}
			}
    } else

      #pragma omp parallel for
			for (int k(0); k < upperElement; k += 1) {
				if (sortedArray[2 * k + 1] > sortedArray[2 * k + 2]) {
					temporary = sortedArray[2 * k + 1];
					sortedArray[2 * k + 1] = sortedArray[2 * k + 2];
					sortedArray[2 * k + 2] = temporary;
			  }
			}
	}
	return sortedArray;
}

int main() {
  
  // ОБЫЧНОЕ - НЕПАРАЛЛЕЛЬНОЕ
  cout << "---------------------------------------------" << '\n';
	cout << "         ОБЫЧНОЕ - НЕПАРАЛЛЕЛЬНОЕ" << '\n';
  cout << "---------------------------------------------" << '\n';

  clock_t start(0), finish(0);

	// double usualUnsortedArray[33] = { 0.5, 10.10, 5.3, 6.4, 2.9, 4.8, 3.10, 5.2, 5.7, 1.9, 89, 121, 12.9, 14.5, 28.9, 0.7, 56, 78.6, 24.5, 44.2, 11, 100, 0, -1, 128, 64, 32, 512, 1024, 55, 67, 32.3, 256 };

  double usualUnsortedArray[1000];
  int A = 1;
  int B = 1000;
 
  for(int i(0); i < 1000; i++) {
    usualUnsortedArray[i] = A + rand() % ((B + 1) - A);
  }

  double parallelUnsortedArray[1000];
    for(int i(0); i < 1000; i++) {
    parallelUnsortedArray[i] = usualUnsortedArray[i];
  }

  cout << "Неотсортированный массив: " << '\n';
	for (int i(0); i < 1000; i += 1) {
		cout << usualUnsortedArray[i] << " ";
	}
  cout << '\n' << '\n';

  start = clock();
	usualBubbleSort(usualUnsortedArray);
  finish = clock();

  cout << "Отсортированный массив: " << '\n';
	for (int i(0); i < 1000; i += 1) {
    cout << usualUnsortedArray[i] << " ";
	}
  cout << '\n';

	cout << "Время выполнения последовательной сортировки: " 
  << (double(finish-start)) << '\n' << '\n';
  //-----------------------------------------------------------------

  // ПАРАЛЛЕЛЬНОЕ
  cout << "---------------------------------------------" << '\n';
	cout << "                 ПАРАЛЛЕЛЬНОЕ" << '\n';
  cout << "---------------------------------------------" << '\n';

  start = 0, finish = 0;
  start = clock();

	// double parallelUnsortedArray[33] = { 0.5, 10.10, 5.3, 6.4, 2.9, 4.8, 3.10, 5.2, 5.7, 1.9, 89, 121, 12.9, 14.5, 28.9, 0.7, 56, 78.6, 24.5, 44.2, 11, 100, 0, -1, 128, 64, 32, 512, 1024, 55, 67, 32.3, 256 };

  // double parallelUnsortedArray[1000];
  // A = 100;
  // B = 1000;
 
  // for(int i(0); i < 1000; i++) {
  //   parallelUnsortedArray[i] = A + rand() % ((B + 1) - A);
  // }

  cout << "Неотсортированный массив: " << '\n';
	for (int i(0); i < 1000; i += 1) {
		cout << parallelUnsortedArray[i] << " ";
	}
  cout << '\n' << '\n';

  start = clock();
	parallelBubbleSort(parallelUnsortedArray);
  finish = clock();

  cout << "Отсортированный массив: " << '\n';
	for (int i(0); i < 1000; i += 1) {
    cout << parallelUnsortedArray[i] << " ";
	}
  cout << '\n';

	cout << "Время выполнения параллельной сортировки: " << (double(finish-start)) << '\n';

	return 0;
}
