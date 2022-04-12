#include <iostream>
#include <sstream>
#include <omp.h>
#include <vector>
#include <time.h>
#include "windows.h"

using namespace std;

static int wheelsChange = 1;
static int tiresChange = 2;

template <typename T>
void outputCritical(int cur_time, T s)
{
#pragma omp critical
    {
        cout << cur_time << ": " << s << '\n';
    }
}

struct Customer
{
    int no;
    bool isServed;
    int inTime;
    int outTime;
    int type;
    
    Customer() {};
    ~Customer() {};
    
    Customer(int t_st, int i)
    {
        no = i;
        inTime = t_st;
        outTime = t_st + 100 + rand() % 300;
        isServed = false;
        type = rand() & 1;
    }
    
    void serve(int& cur_time)
    {
#pragma omp critical
        {
            isServed = true;
        }
        Sleep(450 - type * 300);
        cur_time += 450 - type * 300;
        outputCritical(cur_time, "Клиент " + to_string(no) + " был обслужен");
    }
    
    void whatShouldDo()
    {
        Sleep(inTime);
        
        if (type == wheelsChange)
            outputCritical(inTime, "Клиент " + to_string(no) + " пришел, необходима замена колес");
        else
            outputCritical(inTime, "Клиент " + to_string(no) + " пришел, необходима замена шин");
        
        Sleep(outTime - inTime);
        
        if (!isServed)
            outputCritical(outTime, "Клиент " + to_string(no) + " ушел");
    }
    
};

struct ListOfCustumers
{
    int numberOf;
    vector<Customer> customers;
};

void worker(ListOfCustumers& listOfCustumers)
{
    int currentTime = 0;
    bool isWatchTv = true;
    for (int i(0); i < listOfCustumers.numberOf; i += 1)
    {
        if (listOfCustumers.customers[i].inTime > currentTime)
        {
            if (!isWatchTv)
            {
                isWatchTv = true;
                outputCritical(currentTime, "Отдых, работники смотрят телевизор");
            }
            
            Sleep(listOfCustumers.customers[i].inTime - currentTime);
            currentTime = listOfCustumers.customers[i].inTime;
        }
        if (currentTime >= listOfCustumers.customers[i].inTime && currentTime <= listOfCustumers.customers[i].outTime)
        {
            if (isWatchTv)
            {
                isWatchTv = false;
                outputCritical(currentTime, "Работа в разгаре, телевизор выключен");
            }
            listOfCustumers.customers[i].serve(currentTime);
        }
        if (currentTime > listOfCustumers.customers[i].inTime)
            continue;
    }
    
    outputCritical(currentTime, "Работа на сегодня закончена");
}

int main() {
    
    srand(time(NULL));
    
    ListOfCustumers listOfCustumers;
    cin >> listOfCustumers.numberOf;
    
    listOfCustumers.customers.resize(listOfCustumers.numberOf);
    
    int currentTime = 0;
    
    for (int i(0); i < listOfCustumers.numberOf; i += 1)
    {
        listOfCustumers.customers[i] = Customer(currentTime, i);
        currentTime += 100 + rand() % 900;
    }
    
    omp_set_nested(true);
    omp_set_num_threads(listOfCustumers.numberOf + 10);
    
#pragma omp parallel sections num_threads(listOfCustumers.numberOf + 10)
    {
#pragma omp section
        {
#pragma omp parallel for
            for (int i(0); i < listOfCustumers.numberOf; i += 1)
            {
                listOfCustumers.customers[i].whatShouldDo();
            }
        }
#pragma omp section
        {
            worker(listOfCustumers);
        }
    }
    
    int success;
    cin >> success;
}

