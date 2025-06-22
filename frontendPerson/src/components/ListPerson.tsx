import  { useState } from 'react';
import type { Person, PersonCreate } from '../models/model';
import PersonModal from './PersonModal';
import PersonItem from './PersonItem';

type Props = {
  personTypes: { id: number; description: string }[];
  initialPersons: Person[];
};

function ListPerson({ personTypes, initialPersons }: Props) {
  const [persons, setPersons] = useState<Person[]>(initialPersons);

  const [showModal, setShowModal] = useState(false);
  const [editPersonId, setEditPersonId] = useState<number | null>(null);
  const [form, setForm] = useState<PersonCreate>({
    name: '',
    age: 5,
    personTypeId: personTypes[0].id,
  });

  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

  const openAddModal = () => {
    setForm({ name: '', age: 5, personTypeId: personTypes[0].id });
    setEditPersonId(null);
    setShowModal(true);
  };

  const openEditModal = (person: Person) => {
    setForm({
      name: person.name,
      age: person.age,
      personTypeId: person.personTypeId,
    });
    setEditPersonId(person.id);
    setShowModal(true);
  };

  const handleDelete = async (id: number) => {
    try {
      const response = await fetch(`${apiBaseUrl}/api/persons/${id}`, {
        method: 'DELETE',
      });

      if (response.status === 204) {
        // Remove person from state on success
        setPersons(prev => prev.filter(p => p.id !== id));
      } else {
        const errorText = await response.text();
        alert(`Failed to delete person. Server responded with: ${response.status} - ${errorText}`);
      }
    } catch (error) {
      console.error('Delete error:', error);
      alert('An error occurred while trying to delete the person.');
    }
  };


  const handleSave = (formData: PersonCreate) => {
    const type = personTypes.find(pt => pt.id === formData.personTypeId);
    if (!type) return;

    if (editPersonId === null) {
      // Add
      const newPerson: Person = {
        id: persons.length + 1,
        ...formData,
        personType: type,
      };
      setPersons(prev => [...prev, newPerson]);
    } else {
      // Edit
      setPersons(prev =>
        prev.map(p =>
          p.id === editPersonId
            ? { ...p, ...formData, personType: type }
            : p
        )
      );
    }

    setShowModal(false);
  };

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h1 className="mb-0">List of Persons</h1>
        <button className="btn btn-success" title="Add a person" onClick={openAddModal}>
          <i className="bi bi-plus-lg"></i>
        </button>
      </div>

      {persons.length === 0 ? (
        <p>No persons available.</p>
      ) : (
        <ul className="list-group">
          {persons.map(person => (
            <PersonItem
              key={person.id}
              person={person}
              onEdit={() => openEditModal(person)}
              onDelete={() => handleDelete(person.id)}
            />
          ))}
        </ul>
      )}

      {showModal && (
        <PersonModal
          show={showModal}
          onClose={() => setShowModal(false)}
          onSave={handleSave}
          form={form}
          personTypes={personTypes}
          isEdit={editPersonId !== null}
        />
      )}
    </div>
  );
}

export default ListPerson;
