#include<iostream>
using namespace std;

struct Node
{
	int value;
	Node *next;
};

class stack
{
private:
	Node *head;	
public:
	stack()
	{
		head = NULL;
	}

	void push(int new_value)
	{
		Node *new_node = new Node();

		new_node->value = new_value;
		new_node->next = NULL;

		if (head == NULL)
			head = new_node;
		else
		{
			Node* current_node = head;
			while (current_node->next != NULL)
				current_node = current_node->next;
			
			current_node->next = new_node;
		}
		cout << "Value pushed in your stack: " << new_value << "\n";
	}
	
	int pop()
	{
		if (head == NULL)
			return NULL;
		else
		{
			Node* current_node = head;
			Node* last_node = head;
			int value;
			while (last_node->next != NULL)
			{
				current_node = last_node;
				last_node = last_node->next;
			}

			current_node->next = NULL;

			value = last_node->value;
			delete last_node;			

			cout << "Top value of your stack: " << value << "\n";
			return value;
		}
	}

	void clear()
	{
		if (head == NULL)
			return;
		else
		{
			Node* current_node = head;
			Node* temp;
			while (current_node->next != NULL)
			{
				temp = current_node;
				current_node = current_node->next;
				delete temp;
			}
			head = NULL;
			cout << "Your stack cleared!\n";
		}
	}

	void print()
	{
		if (head == NULL)
			return;
		cout << "\nYour stack:\n";
		Node* current_node = head;
		while (current_node != NULL)
		{
			cout << current_node->value << "\t";
			current_node = current_node->next;
		}
		cout << "\n";
	}
};


void task_1_2_2()
{
	stack stack;

	stack.push(1);
	stack.push(2);
	stack.push(3);
	stack.push(4);
	stack.push(5);
	stack.push(6);

	stack.print();

	stack.pop();
	stack.pop();
	stack.pop();

	stack.print();

	stack.clear();

	stack.print();
}

//int main()
//{
//	task_1_2_2();
//
//	return 1;
//}