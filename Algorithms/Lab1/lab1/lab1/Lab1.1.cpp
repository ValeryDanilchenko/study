//#include <time.h>
//#include<stdlib.h>
//#include<windows.h>
#include<iostream>
#include"Class.h"
using namespace std;


//int GetRandomNumber(int min, int max)
//{	
//	srand(time(NULL));
//	int num = min + rand() % (max - min + 1);
//	Sleep(1000);
//	return num;
//}

int* init_array(int n)
{
	//выделение памяти под каждый элемент кавдратной матрицы
	int* arr = new int[n * n];

	//заполнение матрицы числами
	for (int i = 0; i < n * n; i++)
		arr[i] = i;
	return arr;
}

void swap_array(int* arr, int matrix_size, int line_1, int line_2)
{
	int temp;

	if (line_1 >= matrix_size)
		line_1 = line_1 % matrix_size;
	if (line_2 >= matrix_size)
		line_2 = line_1 % matrix_size;

	//пеерстановка строк
	int j = line_2 * matrix_size;
	for (int i = line_1 * matrix_size; i < line_1 * matrix_size + matrix_size; i++)
	{
		temp = arr[i];
		arr[i] = arr[j];
		arr[j] = temp;
		j++;
	}

}

void print_array(int* arr, int n)
{
	cout << "Your matrix:\n";
	for (int i = 0; i < n * n; i++)
	{		
		cout << arr[i] << "\t";
		if (i % n == n - 1)
			cout << "\n";
	}
}

void task_1_1_1()
{
	cout << "Работа с матрицей представленной в виде массива\n";
	unsigned int n;
	unsigned int line_1, line_2;
	int* arr;
	cout << "Enter square matrix size:\n";
	cin >> n;

	//Создание и заполнение матрицы случайными числами
	arr = init_array(n);

	//вывыод матрицы
	print_array(arr, n);

	cout << "Enter lines you want to swap:\n";
	cin >> line_1 >> line_2;

	//перестановка строк
	swap_array(arr, n, line_1, line_2);

	cout << "__________________________________________\n";
	print_array(arr, n);	
}


////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////


int** init_matrix(int n)
{
	//выделение памяти под массив указателей
	int** arr_p = new int* [n];
	
	//заполнение матрицы числами
	for (int i = 0; i < n; i++)
	{
		int* p = new int[n];
		for (int j = 0; j < n; j++)
			p[j] = i * n + j;

		arr_p[i] = p;
	}

	return arr_p;
}

void swap_matrix(int** arr, int line_1, int line_2)
{
	int* temp;

	//пересстановка строк 
	temp = arr[line_1];
	arr[line_1] = arr[line_2];
	arr[line_2] = temp;
}

void print_matrix(int** arr_p, int n)
{
	cout << "Your matrix:\n";
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < n; j++)
			cout << arr_p[i][j] << "\t";
		cout << "\n";
	}
}

void task_1_1_2()
{
	unsigned int n;
	unsigned int line_1, line_2;
	int** arr_p;

	cout << "Работа с матрицей представленной в виде массива указателей\n";
	cout << "Enter square matrix size:\n";
	cin >> n;
	
	//Создание и заполнение матрицы случайными числами
	arr_p = init_matrix(n);
	
	//вывод матрицы
	print_matrix(arr_p, n);

	cout << "Enter lines you want to swap:\n";
	cin >> line_1 >> line_2;	
	
	if (line_1 >= n)
		line_1 = line_1 % n;
	if (line_2 >= n)
		line_2 = line_1 % n;

	//перестановка строк
	swap_matrix(arr_p, line_1, line_2);

	cout << "__________________________________________\n";
	print_matrix(arr_p, n);
}


int main()
{
	setlocale(LC_ALL, "RU");
	//task_1_1_1();
	//task_1_1_2();

	Class qq;

	qq.foo();

	return 1;
}