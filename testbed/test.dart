class Animal
{
  Animal value() {
    return new Animal();
  }
}

class Dog extends Animal
{
    @override Dog value()
    {
      return new Dog();
    }
}
