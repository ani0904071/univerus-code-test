import React, { useState } from 'react';
import type { Person, PersonType, PersonCreate } from '../models/model';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

function ListPerson() {
  const personTypes: PersonType[] = [
    { id: 1, description: 'Employee' },
    { id: 2, description: 'Teacher' },
  ];

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

  // Modal states
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState<PersonCreate>({
    name: '',
    age: 0,
    personTypeId: personTypes[0].id,
  });

  // Track if modal is for add or edit, and store editing person id
  const [editPersonId, setEditPersonId] = useState<number | null>(null);

  // Open modal for adding person
  const openAddModal = () => {
    setForm({ name: '', age: 0, personTypeId: personTypes[0].id });
    setEditPersonId(null);
    setShowModal(true);
  };

  // Open modal for editing person (on View)
  const openEditModal = (person: Person) => {
    setForm({
      name: person.name,
      age: person.age,
      personTypeId: person.personTypeId,
    });
    setEditPersonId(person.id);
    setShowModal(true);
  };

  const handleDelete = (id: number) => {
    setPersons(prev => prev.filter(p => p.id !== id));
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: name === 'age' || name === 'personTypeId' ? parseInt(value) : value,
    }));
  };

  // Add or update person on modal submit
  const handleSave = () => {
    const type = personTypes.find(pt => pt.id === form.personTypeId);
    if (!type) return;

    if (editPersonId === null) {
      // Add mode
      const newPerson: Person = {
        id: persons.length + 1, // or use UUID
        name: form.name,
        age: form.age,
        personTypeId: form.personTypeId,
        personType: type,
      };
      setPersons(prev => [...prev, newPerson]);
    } else {
      // Edit mode
      setPersons(prev =>
        prev.map(p =>
          p.id === editPersonId
            ? { ...p, name: form.name, age: form.age, personTypeId: form.personTypeId, personType: type }
            : p
        )
      );
    }
    setShowModal(false);
  };

  return (
    <div className="container mt-4">
      {/* Header with Add Button */}
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h1 className="mb-0">List of Persons</h1>
        <button type="button" className="btn btn-success" title="Add a person" onClick={openAddModal}>
          <i className="bi bi-plus-lg"></i>
        </button>
      </div>

      {/* List */}
      {persons.length === 0 ? (
        <p>No persons available.</p>
      ) : (
        <ul className="list-group">
          {persons.map(person => (
            <li
              key={person.id}
              className="list-group-item list-group-item-action d-flex justify-content-between align-items-center"
            >
              <div>
                <strong>{person.name}</strong> (Age: {person.age}) - {person.personType.description}
              </div>
              <div>
                <button
                  className="btn btn-primary btn-sm me-2"
                  onClick={() => openEditModal(person)} // Open modal to edit on View click
                >
                  View
                </button>
                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(person.id)}>
                  Delete
                </button>
              </div>
            </li>
          ))}
        </ul>
      )}

      {/* Modal */}
      {showModal && (
        <div className="modal show d-block" tabIndex={-1}>
          <div className="modal-dialog">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">{editPersonId === null ? 'Add a Person' : 'Edit Person'}</h5>
                <button type="button" className="btn-close" onClick={() => setShowModal(false)}></button>
              </div>
              <div className="modal-body">
                <div className="mb-3">
                  <label className="form-label">Name</label>
                  <input
                    name="name"
                    type="text"
                    className="form-control"
                    value={form.name}
                    onChange={handleChange}
                  />
                </div>
                <div className="mb-3">
                  <label className="form-label">Age</label>
                  <input
                    name="age"
                    type="number"
                    className="form-control"
                    value={form.age}
                    onChange={handleChange}
                  />
                </div>
                <div className="mb-3">
                  <label className="form-label">Person Type</label>
                  <select
                    name="personTypeId"
                    className="form-select"
                    value={form.personTypeId}
                    onChange={handleChange}
                  >
                    {personTypes.map(pt => (
                      <option key={pt.id} value={pt.id}>
                        {pt.description}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
              <div className="modal-footer">
                <button className="btn btn-secondary" onClick={() => setShowModal(false)}>
                  Cancel
                </button>
                <button className="btn btn-primary" onClick={handleSave}>
                  {editPersonId === null ? 'Add' : 'Save'}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default ListPerson;
