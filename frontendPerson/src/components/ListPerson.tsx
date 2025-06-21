import React, { useState } from 'react';
import type { Person, PersonType } from '../models/model';

function ListPerson() {
  const personTypes: PersonType[] = [
    { id: 1, description: 'Employee' },
    { id: 2, description: 'Teacher' },
  ];
  let selectedIndex = 0;

  const [persons, setPersons] = useState<Person[]>([
    {
      id: 1,
      name: 'John Doe',
      age: 30,
      personTypeId: 1,
      personType: personTypes[0],
    },
    {
      id: 2,
      name: 'Jane Smith',
      age: 25,
      personTypeId: 2,
      personType: personTypes[1],
    },
  ]);

  const handleDelete = (id: number) => {
    setPersons(prev => prev.filter(p => p.id !== id));
  };

  const handleView = (person: Person) => {
    alert(`Name: ${person.name}\nAge: ${person.age}\nType: ${person.personType.description}`);
  };

  return (
    <div className="container mt-4">
      <h1>List of Persons</h1>
      <ul className="list-group">
        {persons.map((person, index) => (
          <li key={person.id} className="list-group-item d-flex justify-content-between align-items-center">
            <div>
              <strong>{person.name}</strong> (Age: {person.age}) - {person.personType.description}
            </div>
            <div>
              <button className="btn btn-primary btn-sm me-2" onClick={() => handleView(person)}>View</button>
              <button className="btn btn-danger btn-sm" onClick={() => handleDelete(person.id)}>Delete</button>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default ListPerson;
