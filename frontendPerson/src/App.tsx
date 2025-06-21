import './App.css';
import ListPerson from './components/ListPerson';
import type { Person, PersonType } from './models/model';

function App() {
  const personTypes: PersonType[] = [
    { id: 1, description: 'Teacher' },
    { id: 2, description: 'Studnet' },
  ];

  const initialPersons: Person[] = [
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
  ];

  return (
    <div>
      <ListPerson personTypes={personTypes} initialPersons={initialPersons} />
    </div>
  );
}

export default App;
