#include<iostream>
#define SIZE 10
using namespace std;

class stack
{
private:
	int arr[SIZE] = {};
	int top = -1;
public:
	void push(int value)
	{
		if (top + 1 >= SIZE)
		{
			cout << "Error! Stack overflow!!\n";
			return;
		}
		
		arr[top + 1] = value;
		top++;		
		cout << "Value pushed in your stack: " << value << "\n";
	}

	int pop()
	{
		if (top == -1)
		{
			cout << "Error! Stack is empty!!\n";
			return 0;
		}

		int value = arr[top];
		top--;

		cout << "Top value of your stack: " << value << "\n";
		return value;
	}


	void clear()
	{
		for (size_t i = 0; i < SIZE; i++)
		{
			arr[i] = NULL;
		}
		top = -1;

		cout << "Your stack cleared!\n";
	}

	void print()
	{
		if (top == -1)		
			cout << "Stack is empty!!\n";
		else
		{
			cout << "Your stack:\n";
			for (int i = 0; i < top + 1; i++)
				cout << arr[i] << "\t";		
			cout << "\n";
		}
	}

};

void task_1_2_1()
{
	stack item;

	item.push(1);
	item.push(2);
	item.push(3);
	item.push(4);
	item.push(5);
	item.push(6);
	item.push(7);
	item.push(8);
	item.push(9);
	item.push(10);
	item.push(11);
	
	cout << "\n\n";
	item.print();
	cout << "\n\n";

	item.pop();
	item.pop();
	item.pop();
	item.pop();
	item.pop();
	item.pop();

	cout << "\n\n";
	item.print();
	cout << "\n\n";


	item.push(6);
	item.push(7);
	item.push(8);
	item.push(9);
	item.push(10);
	item.push(11);

	cout << "\n\n";
	item.clear();

	item.print();
}


//int main()
//{
//	task_1_2_1();
//
//	return 1;
//}
