#include<iostream>
#define SIZE 10
using namespace std;

class queue
{
private:
	int q[SIZE] = {};
	int rear, front;

public:
	//Создание
	queue()
	{		
		rear = 0;
		front = 1;
	}

	//Добавление
	void push(int value)
	{
		if (rear < SIZE - 1)
		{
			rear++;
			q[rear] = value;
			cout << "Value pushed in your queue: " << value << "\n";
			return;
		}
		cout << "Queue is overflowed!\n";
	}

	bool empty()
	{
		if (rear < front)
			return true;
		else
			return false;
	}

	//Извлечение
	int pop()
	{
		if (this->empty())
		{
			cout << "Queue is empty!\n";
			return 0;
		}

		int value = q[front];
		for (int i = front; i < rear; i++)
			q[i] = q[i + 1];

		rear--;
		cout << "Front value of your queue: " << value << "\n";
		return value;
	}

	//Отчистка
	void clear()
	{
		for (size_t i = 0; i < SIZE; i++)
		{
			q[i] = NULL;
		}
		rear = 0;
		front = 1;
		cout << "Your queue cleared!\n";
	}

	void print()
	{
		if (this->empty())
			cout << "Queue is empty!\n";
		else
		{
			cout << "Your queue:\n";
			for (int i = front; i < rear; i++)
				cout << q[i] << "\t";
			cout << "\n";
		}
	}
};


void task_1_3_1()
{
	//Создание
	queue q;

	//Добавление
	q.push(1);
	q.push(2);
	q.push(3);
	q.push(4);
	q.push(5);
	q.push(6);
	q.push(7);
	q.push(8);
	q.push(9);
	q.push(10);
	q.push(11);

	cout << "\n\n";
	q.print();
	cout << "\n\n";

	//Извлечение
	q.pop();
	q.pop();
	q.pop();
	q.pop();

	cout << "\n\n";
	q.print();
	cout << "\n\n";

	//Отчистка
	q.clear();
}

//int main()
//{
//	task_1_3_1();
//
//	return 1;
//}