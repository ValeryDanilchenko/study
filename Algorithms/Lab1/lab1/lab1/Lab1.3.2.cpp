#include<iostream>
using namespace std;

struct Node
{
	int value;
	Node* next;
	Node* prev;

	//Создание
	Node()
	{
		value = NULL;
		next = this;
		prev = this;
	}
};


class Queue
{
private:
	Node* head;
public:
	Queue()
	{
		head = NULL;
	}

	//Добавление
	void push(int new_value)
	{
		Node* new_node = new Node();
		new_node->value = new_value;
		

		if (head == NULL)
			head = new_node;
		else
		{
			new_node->prev = head->prev;
			head->prev->next = new_node;
			head->prev = new_node;
			new_node->next = head;
		}
		cout << "Value pushed in your queue: " << new_value << "\n";
	}

	bool empty()
	{
		if (head == NULL)
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

		int value = head->value;

		Node* temp = head;
		head = head->next;
		temp->prev->next = head;
		head->prev = temp->prev;

		delete temp;		
		
		cout << "Front value of your queue: " << value << "\n";
		return value;
	}

	//Отчистка
	void clear()
	{
		if (this->empty())
			return;
		else
		{
			Node* current_node = head;
			Node* temp;
			while (1)
			{
				temp = current_node;
				current_node = current_node->next;
				delete temp;
				if ((current_node == head))
					break;
			}
			head = NULL;
			cout << "Your queue cleared!\n";
		}
	}


	void print()
	{
		if (this->empty())
			return;
		cout << "Your queue:\n";
		Node* current_node = head;
		while (1)
		{
			cout << current_node->value << "\t";
			current_node = current_node->next;
			if ((current_node == head))
				break;
		}
		cout << "\n";
	}
};

void task_1_3_2()
{
	//Создание
	Queue list; 

	//Добавление
	list.push(1);
	list.push(2);
	list.push(3);
	list.push(4);
	list.push(5);
	list.push(6);

	cout << "\n\n";
	list.print();
	cout << "\n\n";

	//Извлечение
	list.pop(); 
	list.pop();
	list.pop();

	cout << "\n\n";
	list.print();
	cout << "\n\n";

	//Отчистка
	list.clear();
}



//int main()
//{
//	task_1_3_2();
//
//	return 1;
//}