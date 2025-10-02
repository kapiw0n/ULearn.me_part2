<h3>1. Что вы можете сказать о словаре Dictionary?</h3>
--- Словарь позволяет эффективно проверить, содержит ли он ключ<br>
--- Для каждого ключа словарь хранит только одно значение

<h3>2. Выберите корректные варианты реализации GetHashCode. Считайте, что Name, Surname, Patronymic не могут быть null</h3>
--- return 42<br>
--- return Surname.GetHashCode()<br>
--- return Surname.GetHashCode() * 31 + Name.GetHashCode()<br>
--- return (Surname.GetHashCode() * 31 + Name.GetHashCode()) * 31 + Patronymic.GetHashCode()
