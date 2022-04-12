#include <iostream>
#include <fstream>
#include <sstream>
#include <algorithm>
#include <vector>
#include <omp.h>
#include <random>
#include <time.h>

using namespace std;

vector<vector<double>> readMatrixFromCsvFile(string fileName)
{
	ifstream indata;
	vector<vector<double>> matrix;
	indata.open(fileName);
	string line; 
	
	int i(0); int j(0);
	
	while (getline(indata,line)) {
		stringstream lineStream(line);
		string cell;
		vector<double> temp;
		while (getline(lineStream, cell, ',')) {
			temp.push_back(stod(cell));
		}
		matrix.push_back(temp);
	}
	return matrix;
}

vector<vector<double>> multiplyMatrixFromCsv(vector<vector<double>> m1, vector<vector<double>> m2, int size) {

	vector<vector<double>> result(size);
  #pragma omp parallel for
  for(int i(0); i < size; i += 1)
	{
		for (int j(0); j < size; j += 1)
		{
			double temp(0);
			for (int k(0); k < size; k += 1)
			{
				temp += m2[k][j] * m1[i][k];
			}
			result[i].insert(result[i].begin(), temp);
		}
	}
	return result;
}

void matrixMultiplyWithRandom(double** m1, double** m2, double** result, int size)
{
  #pragma omp parallel for
  for(int i(0); i < size; i += 1)
	{
		for (int j(0); j < size; j += 1)
		{
			double temp(0);
			for (int k(0); k < size; k += 1)
			{
				temp += m2[k][j] * m1[i][k];
			}
			result[i][j] = temp;
		}
	}
}
double** createNewMatrix(int size)
{
	double** m = new double*[size];
	for (int i = 0; i < size; i++)
	{
		m[i] = new double[size];
	}
	return m;
}

template <class Generator>
double** initializeMatrix(double** m, int size, Generator& gen)
{
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			m[i][j] = static_cast<double>(gen());
		}
	}
	return m;
}

void printMatrix(double** m, int size) {
	for (int i(0); i < size; i += 1) {
		for (int j(0); j < size; j += 1) {
			cout << m[i][j] << " ";
		}
		cout << '\n';
	}
}

int main() {
	const int size = 10;
  string pathToMatrix = "in.csv";
  clock_t start(0), finish(0);

  start = clock();

  // €—€‹Ž: Œ€’ˆ–€, ‘—ˆ’€‚€…Œ€Ÿ ˆ‡ ”€‰‹€
	vector<vector<double>> lines = readMatrixFromCsvFile(pathToMatrix);
	vector<vector<double>> resultOfMultiply = multiplyMatrixFromCsv(lines,lines, size);

  finish = clock();

	for (int i(0); i < size; i += 1) {
		for (int j(0); j < size; j += 1) {
			cout <<resultOfMultiply[i][j] << " ";
		}
		cout << '\n';
	}
  cout << '\n';
  cout << "‚ðåìß ïåðåìíîæåíèß ìàòðèöû èç CSV ôàéëà: " 
  << (double(finish-start) / CLOCKS_PER_SEC) << " ñåêóíä" << '\n' << '\n';

  // ŠŽ…–: Œ€’ˆ–€, ‘—ˆ’€‚€…Œ€Ÿ ˆ‡ ”€‰‹€

  // €—€‹Ž: Œ€’ˆ–€, ‡€„€‚€…Œ€Ÿ ‘‹“—€‰›Œˆ —ˆ‘‹€Œˆ
  start = 0, finish = 0;
  start = clock();

	double** firstMatrix = initializeMatrix(createNewMatrix(size), size, rand);
	double** secondMatrix = initializeMatrix(createNewMatrix(size), size, rand);
	double** result = createNewMatrix(size);

	matrixMultiplyWithRandom(firstMatrix, secondMatrix, result, size);
  finish = clock();

	printMatrix(result, size);
  cout << '\n';

  cout << "‚ðåìß ïåðåìíîæåíèß ìàòðèöû, çàïîëíåííîé ñëó÷àéíûìè ÷èñëàìè: " 
  << (double(finish-start) / CLOCKS_PER_SEC) << " ñåêóíä" << '\n';
  // ŠŽ…–: Œ€’ˆ–€, ‡€„€‚€…Œ€Ÿ ‘‹“—€‰›Œˆ —ˆ‘‹€Œˆ
	
	return 0;
}