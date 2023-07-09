#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
using namespace std;



struct Node
{
public:
	char value;
	int next;
	int ptr;


	Node()
	{
		value = NULL;
		next = 0;
		ptr = 0;
	}

	Node(char value)
	{
		this->value = value;
		next = NULL;
		ptr = NULL;
	}

};


class Graph
{
public:
	Node *head;
	//int* head1;
	static int back_prod;
	static int back_comp;


	Graph()
	{		
		//int arr[1] = {};
		//head1 = arr;
		head = new Node();
		FILE* f = fopen("products.bin", "wb");
		fwrite(&head, sizeof(Node), 1, f);
		fclose(f);

		FILE* f1 = fopen("components.bin", "wb");
		fwrite(&head, sizeof(Node), 1, f1);
		fclose(f1);
	}
	

	Node* init_node(FILE* f, int ptr)
	{
		Node* node;
		fseek(f,ptr, SEEK_SET);
		fread(&node, sizeof(Node), 1, f);
		return node;
	}

	void insert_product(char value)
	{
		FILE* f = fopen("products.bin", "ab+");
		Node *new_node = new Node(value);
		fseek(f, 0, SEEK_SET);
		fread(&head, sizeof(Node), 1, f);


		if (head->ptr == 0)
		{
			fseek(f, 0, SEEK_END);
			fwrite(&new_node, sizeof(Node), 1, f);
			back_prod++;

			if (head->next == 0)
				head->next = back_prod;
			else
			{
				Node* current_node = init_node(f, head->next * sizeof(Node));
				/*fseek(f, head->next * sizeof(Node), SEEK_SET);
				fread(&current_node, sizeof(Node), 1, f);*/
				while (current_node->next != NULL)
				{						
					fseek(f, (current_node->next) * sizeof(Node), SEEK_SET);
					fread(&current_node, sizeof(Node), 1, f);
				}
				current_node->next = back_prod;
			}
		}
		else
		{
			Node* temp;
			int head_new_ptr;
			fseek(f, (head->ptr) * sizeof(Node), SEEK_SET);
			fread(&temp, sizeof(Node), 1, f);
			head_new_ptr = temp->ptr;


			Node* deleted_node;
			fseek(f, head->ptr * sizeof(Node), SEEK_SET);
			fread(&deleted_node, sizeof(Node), 1, f);
			*deleted_node = *new_node;

			if (head->next == 0)
			{
				head->next = head->ptr;
				head->ptr = head_new_ptr;
			}
			else
			{
				Node* current_node;
				fseek(f, head->next * sizeof(Node), SEEK_SET);
				fread(&current_node, sizeof(Node), 1, f);
				while (current_node->next != NULL)
				{
					fseek(f, (current_node->next) * sizeof(Node), SEEK_SET);
					fread(&current_node, sizeof(Node), 1, f);
				}


				current_node->next = head->ptr;
				head->ptr = head_new_ptr;
			}

		}
		fclose(f);
	}

	void delete_product(char value)
	{
		FILE* f = fopen("products.bin", "ab+");

		fseek(f, 0, SEEK_SET);
		fread(&head, sizeof(Node), 1, f);

		Node* current_node;
		Node* prev_node = head;	
		fseek(f, head->next * sizeof(Node), SEEK_SET);
		fread(&current_node, sizeof(Node), 1, f);

		while (current_node->value != value)
		{
			prev_node = current_node;
			fseek(f, (current_node->next) * sizeof(Node), SEEK_SET);
			fread(&current_node, sizeof(Node), 1, f);
		}

		current_node->ptr = head->ptr;
		head->ptr = prev_node->next;
		prev_node->next = current_node->next;
		current_node->next = NULL;
		current_node->value = NULL;
		 
	}


	void insert_link(char prod_elem, char comp_elem, char linked_elem)
	{
		FILE* f1 = fopen("products.bin", "ab+");
		FILE* f2 = fopen("components.bin", "ab+");
		Node* new_comp_node = new Node(comp_elem);
		fseek(f1, 0, SEEK_SET);
		fread(&head, sizeof(Node), 1, f1);


		Node* current_node;
		fseek(f1, head->next * sizeof(Node), SEEK_SET);
		fread(&current_node, sizeof(Node), 1, f1);
		while (current_node->value != prod_elem)
		{
			fseek(f1, (current_node->next) * sizeof(Node), SEEK_SET);
			fread(&current_node, sizeof(Node), 1, f1);
		}

		Node* linked_node;
		int linked_node_ptr;
		fseek(f1, head->next * sizeof(Node), SEEK_SET);
		fread(&linked_node, sizeof(Node), 1, f1);
		while (linked_node->value != linked_elem)
		{
			linked_node_ptr = linked_node->next;
			fseek(f1, (linked_node->next) * sizeof(Node), SEEK_SET);
			fread(&linked_node, sizeof(Node), 1, f1);
		}

		fseek(f2, 0, SEEK_SET);
		fread(&head, sizeof(Node), 1, f2);

		if (current_node->ptr == 0)
		{

			if (head->ptr == 0)
			{

				fseek(f2, 0, SEEK_END);
				fwrite(&new_comp_node, sizeof(Node), 1, f2);
				back_comp++;
				current_node->ptr = back_comp;			

			}
			else
			{
				Node* temp;
				int head_new_ptr;
				fseek(f2, (head->ptr) * sizeof(Node), SEEK_SET);
				fread(&temp, sizeof(Node), 1, f2);
				head_new_ptr = temp->ptr;

				Node* deleted_node;
				fseek(f2, head->ptr * sizeof(Node), SEEK_SET);
				fread(&deleted_node, sizeof(Node), 1, f2);
				*deleted_node = *new_comp_node;

				current_node->next = head->ptr;
				head->ptr = head_new_ptr;
			}
		}
		else
		{
			Node* prev_comp_node;
			fseek(f2, current_node->ptr * sizeof(Node), SEEK_SET);
			fread(&prev_comp_node, sizeof(Node), 1, f2);
			while (prev_comp_node->next != NULL)
			{
				fseek(f2, (prev_comp_node->next) * sizeof(Node), SEEK_SET);
				fread(&prev_comp_node, sizeof(Node), 1, f2);
			}

			if (head->ptr == 0)
			{

				fseek(f2, 0, SEEK_END);
				fwrite(&new_comp_node, sizeof(Node), 1, f2);
				back_comp++;				

				prev_comp_node->next = back_comp;
			}
			else
			{
				Node* temp;
				int head_new_ptr;
				fseek(f2, (head->ptr) * sizeof(Node), SEEK_SET);
				fread(&temp, sizeof(Node), 1, f2);
				head_new_ptr = temp->ptr;

				Node* deleted_node;
				fseek(f2, head->ptr * sizeof(Node), SEEK_SET);
				fread(&deleted_node, sizeof(Node), 1, f2);
				*deleted_node = *new_comp_node;

				prev_comp_node->next = head->ptr;
				head->ptr = head_new_ptr;
			}
		}

		new_comp_node->ptr = linked_node_ptr;
		fclose(f1);
		fclose(f2);
	}

	void delete_comp(char comp_elem)
	{
		FILE* f = fopen("components.bin", "ab+");

		fseek(f, 0, SEEK_SET);
		fread(&head, sizeof(Node), 1, f);

		Node* current_node;
		Node* prev_node = head;
		fseek(f, head->next * sizeof(Node), SEEK_SET);
		fread(&current_node, sizeof(Node), 1, f);

		while (current_node->value != comp_elem)
		{
			prev_node = current_node;
			fseek(f, (current_node->next) * sizeof(Node), SEEK_SET);
			fread(&current_node, sizeof(Node), 1, f);
		}

		current_node->ptr = head->ptr;
		head->ptr = prev_node->next;
		prev_node->next = current_node->next;
		current_node->next = NULL;
		current_node->value = NULL;
	}

	void print_graph()
	{
		FILE* f1 = fopen("products.bin", "ab+");
		FILE* f2 = fopen("components.bin", "ab+");
		
		fseek(f1, 0, SEEK_SET);
		fread(&head, sizeof(Node), 1, f1);


		Node* current_node;
		fseek(f1, head->next * sizeof(Node), SEEK_SET);
		fread(&current_node, sizeof(Node), 1, f1);
		while (current_node->next != NULL)
		{
			
			cout << current_node->value << "\n";
			
			if (current_node->ptr != 0)
			{
				fseek(f2, 0, SEEK_SET);
				fread(&head, sizeof(Node), 1, f2);

				Node* comp_node;
				fseek(f2, head->next * sizeof(Node), SEEK_SET);
				fread(&comp_node, sizeof(Node), 1, f2);
				while (1)
				{
					
					Node* linked_node;
					fseek(f1, comp_node->ptr * sizeof(Node), SEEK_SET);
					fread(&linked_node, sizeof(Node), 1, f1);
					
					cout<<'\t' << linked_node->value << ":" << (int)comp_node->value << "\n";

					fseek(f2, (comp_node->next) * sizeof(Node), SEEK_SET);
					fread(&comp_node, sizeof(Node), 1, f2);

					if (comp_node == head)
						break;
				}
			}

			fseek(f1, (current_node->next) * sizeof(Node), SEEK_SET);
			fread(&current_node, sizeof(Node), 1, f1);
		}

		fclose(f1);
		fclose(f2);
	}


	void print()
	{
		cout << "_________________\n";
		FILE* f = fopen("products.bin", "rb");
		Node* node = new Node();
		while(fread(&node, sizeof(Node), 1, f))
			cout << node->value << '\t' << node->next << '\t' << node->ptr << '\n';

		cout << "_________________\n";
	}

	void print_comp()
	{
		cout << "++++++++++++++++++\n";
		FILE* f = fopen("components.bin", "rb");
		Node* node = new Node();
		while (fread(&node, sizeof(Node), 1, f))
			cout << (int)node->value << '\t' << node->next << '\t' << node->ptr << '\n';

		cout << "++++++++++++++++++\n";
	}
	
};int Graph::back_prod = 0;
int Graph::back_comp = 0;

void test()
{

	Graph graph;

	graph.print();

 	graph.insert_product('a');
	graph.print();

	graph.insert_product('b');
	graph.print();

	graph.insert_product('c');
	graph.print();


	graph.insert_product('d');
	graph.print();


	graph.insert_product('e');
	graph.print();

	graph.insert_product('f');
	graph.print();

	graph.insert_product('g');
	graph.print();

	//graph.delete_product('a');
	//graph.delete_product('d');
	//graph.delete_product('c');
	//graph.print();


	graph.insert_product('h');
	graph.print();

	graph.insert_product('i');
	graph.print();


	graph.insert_link('a', 3, 'i');
	graph.print();
	graph.print_comp();


	graph.insert_link('a', 4, 'd');
	graph.print();
	graph.print_comp();


	graph.insert_link('a', 1, 'e');
	graph.print();
	graph.print_comp();

	graph.delete_comp(4);
	graph.print_comp();



	cout << "\n\n\n";

	graph.print_graph();

}

int main()
{
	test();


	return 1;
}