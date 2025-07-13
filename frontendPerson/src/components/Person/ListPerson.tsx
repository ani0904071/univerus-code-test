import { useMemo, useState } from "react";
import type { Person, PersonCreate } from "../../models/model";
import PersonModal from "../../modals/PersonModal";
import {
  type SortKey,
  type SortDirection,
  sortPersons,
} from "../../utils/sortPersons";
import styles from "./ListPerson.module.css";
import personService from "../../services/personService";

type Props = {
  personTypes: { id: number; description: string }[];
  initialPersons: Person[];
};

function ListPerson({ personTypes, initialPersons }: Props) {
  const [persons, setPersons] = useState<Person[]>(initialPersons);
  const [sortKey, setSortKey] = useState<SortKey>("age");
  const [sortDirection, setSortDirection] = useState<SortDirection>("asc");

  const [showModal, setShowModal] = useState(false);
  const [editPersonId, setEditPersonId] = useState<number | null>(null);
  const [form, setForm] = useState<PersonCreate>({
    name: "",
    age: 5,
    personTypeId: personTypes[0].id,
  });

  const sortedPersons = useMemo(() => {
    return sortPersons(persons, sortKey, sortDirection);
  }, [persons, sortKey, sortDirection]);

  const handleSort = (key: SortKey) => {
    if (key === sortKey) {
      setSortDirection((prev) => (prev === "asc" ? "desc" : "asc"));
    } else {
      setSortKey(key);
      setSortDirection("asc");
    }
  };

  const openAddModal = () => {
    setForm({ name: "", age: 5, personTypeId: personTypes[0].id });
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
      const success = await personService.delete(id);
      if (success) {
        setPersons((prev) => prev.filter((p) => p.id !== id));
      } else {
        alert("Failed to delete person.");
      }
    } catch (error) {
      console.error("Delete error:", error);
      alert("An error occurred while deleting.");
    }
  };

  const handleSave = async (formData: PersonCreate) => {
    const type = personTypes.find((pt) => pt.id === formData.personTypeId);
    if (!type) return;

    try {
      if (editPersonId === null) {
        const newPerson = await personService.create(formData);
        if (!newPerson) return alert("Failed to create person.");
        newPerson.personType = type;
        setPersons((prev) => [...prev, newPerson]);
      } else {
        const success = await personService.update(editPersonId, formData);
        if (!success) return alert("Failed to update person.");
        setPersons((prev) =>
          prev.map((p) =>
            p.id === editPersonId ? { ...p, ...formData, personType: type } : p
          )
        );
      }

      setShowModal(false);
    } catch (error) {
      console.error("Save error:", error);
      alert("An error occurred while saving.");
    }
  };

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h1 className="mb-0">List of Persons</h1>
        <button
          className="btn btn-success"
          title="Add a person"
          onClick={openAddModal}
        >
          <i className="bi bi-plus-lg"></i>
        </button>
      </div>

      <table className="table table-bordered table-hover">
        <thead className="table-light">
          <tr>
            <th
              onClick={() => handleSort("name")}
              style={{ cursor: "pointer" }}
            >
              Name{" "}
              {sortKey === "name" ? (sortDirection === "asc" ? "↑" : "↓") : ""}
            </th>
            <th onClick={() => handleSort("age")} style={{ cursor: "pointer" }}>
              Age{" "}
              {sortKey === "age" ? (sortDirection === "asc" ? "↑" : "↓") : ""}
            </th>
            <th
              onClick={() => handleSort("personTypeId")}
              style={{ cursor: "pointer" }}
            >
              Type{" "}
              {sortKey === "personTypeId"
                ? sortDirection === "asc"
                  ? "↑"
                  : "↓"
                : ""}
            </th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {sortedPersons.length === 0 ? (
            <tr>
              <td colSpan={4}>No persons available.</td>
            </tr>
          ) : (
            sortedPersons.map((person) => (
              <tr key={person.id}>
                <td>
                  <span
                    className={styles["truncate-hover"]}
                    data-fulltext={person.name}
                    style={{ maxWidth: "120px" }}
                    title={person.name}
                  >
                    {person.name}
                  </span>
                </td>
                <td>{person.age}</td>
                <td>
                  <span
                    className={styles["truncate-hover"]}
                    style={{ maxWidth: "120px" }}
                    data-fulltext={person.personType?.description || ""}
                    title={person.personType?.description}
                  >
                    {person.personType?.description || person.personTypeId}
                  </span>
                </td>
                <td>
                  <button
                    className="btn btn-sm btn-primary me-2"
                    onClick={() => openEditModal(person)}
                  >
                    Edit
                  </button>
                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => handleDelete(person.id)}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>

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
