#include <iostream>
using namespace std;

struct Node
{
	int value;
	unsigned char height;
	Node* left;
	Node* right;

	Node(int new_value) 
	{
		value = new_value;
		left = nullptr;
		right = nullptr;
		height = 1;
	}
};

unsigned char height(Node* current_node)
{
	if (current_node != NULL)
		return current_node->height;	
	return 0;
}

//вычисление balance factor
int balance_factor(Node* current_node)
{
	return height(current_node->right) - height(current_node->left);
}

//коррекция height для данного узла
void fix_height(Node* current_node)
{
	unsigned char hl = height(current_node->left);
	unsigned char hr = height(current_node->right);
	if (hl > hr)
		current_node->height = hl + 1;
	else
		current_node->height = hr + 1;
}

// правый поворот вокруг p
Node* rotate_right(Node* p) 
{
	Node* q = p->left;
	p->left = q->right;
	q->right = p;
	fix_height(p);
	fix_height(q);
	return q;
}

// левый поворот вокруг q
Node* rotate_left(Node* p)
{
	Node* q = p->right;
	p->right = q->left;
	q->left = p;
	fix_height(p);
	fix_height(q);
	return q;
}

// балансировка узла
Node* balance(Node* p) 
{
	fix_height(p);
	if (balance_factor(p) == 2)
	{
		if (balance_factor(p->right) < 0)
			p->right = rotate_right(p->right);
		return rotate_left(p);
	}
	if (balance_factor(p) == -2)
	{
		if (balance_factor(p->left) > 0)
			p->left = rotate_left(p->left);
		return rotate_right(p);
	}
	return p;
}

// вставка нового числа в балансированное дерево
Node* insert_balanced(int new_value, Node*& current_node) 
{
	if (!current_node) 
		return new Node(new_value);
	if (new_value < current_node->value)
		current_node->left = insert_balanced(new_value, current_node->left);
	else if (new_value > current_node->value)
		current_node->right = insert_balanced(new_value, current_node->right);
	return balance(current_node);
}


///////////////////////////////////////////////////////
///////////////////////////////////////////////////////

// вставка нового числа в небалансированное дерево
Node* insert(int new_value, Node*& current_node)
{		
	if (!current_node)
		return new Node(new_value);
	if (new_value < current_node->value)
		current_node->left = insert(new_value, current_node->left);
	else if (new_value > current_node->value)
		current_node->right = insert(new_value, current_node->right);
	return current_node;
}

//отчистка (постфиксный обход дерева)
void clear(Node*& current_node)
{
	if (current_node)
	{
		clear(current_node->left);
		clear(current_node->right);
		delete current_node;
		current_node = NULL;
	}
}

//вывод дерева(инфиксный обход дерева)
void print_tree(Node*& current_node)
{
	if (current_node)
	{
		print_tree(current_node->left);
		cout << current_node->value << "\t";
		//cout << current_node->value << " height: " << (int)current_node->height << "\t";
		print_tree(current_node->right);
	}
}


void task_2_1()
{
	Node* tree = NULL;	

	tree = insert(4, tree);
	tree = insert(3, tree);
	tree = insert(1, tree);
	tree = insert(5, tree);
	tree = insert(7, tree);
	tree = insert(8, tree);
	tree = insert(12, tree);
	tree = insert(2, tree);
	tree = insert(6, tree);
	tree = insert(11, tree);

	cout << "Imbalanced tree:\n";
	print_tree(tree);
	cout << "\n\n";

	clear(tree);

	for (int i = 0; i < 10; i++)
		tree = insert_balanced(i, tree);
	
	cout << "Balanced tree:\n";
	print_tree(tree);
	clear(tree);
}

//int main()
//{
//	task_2_1();
//	return 1;
//}