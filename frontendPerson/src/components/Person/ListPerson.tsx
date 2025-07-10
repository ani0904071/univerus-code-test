import { useMemo, useState } from "react";
import type { Person, PersonCreate } from "../../models/model";
import PersonModal from "../../modals/PersonModal";
import {
  type SortKey,
  type SortDirection,
  sortPersons,
} from "../../utils/sortPersons";
import styles from "./ListPerson.module.css";

type Props = {
  personTypes: { id: number; description: string }[];
  initialPersons: Person[];
};

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

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
      const response = await fetch(`${apiBaseUrl}/api/v1/persons/${id}`, {
        method: "DELETE",
      });

      if (response.status === 204) {
        setPersons((prev) => {
          const list = [...prev];
          const idx = list.findIndex((p) => p.id === id);
          if (idx !== -1) {
            list.splice(idx, 1);
          }
          return list;
        });
      } else {
        const errorText = await response.text();
        alert(
          `Failed to delete person. Server responded with: ${response.status} - ${errorText}`
        );
      }
    } catch (error) {
      console.error("Delete error:", error);
      alert("An error occurred while trying to delete the person.");
    }
  };

  const handleSave = async (formData: PersonCreate) => {
    try {
      const type = personTypes.find((pt) => pt.id === formData.personTypeId);
      if (!type) return;

      if (editPersonId === null) {
        const response = await fetch(`${apiBaseUrl}/api/v1/persons`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(formData),
        });

        if (!response.ok) {
          const errorText = await response.text();
          alert(`Failed to create person: ${response.status} - ${errorText}`);
          return;
        }

        const newPerson: Person = await response.json();
        newPerson.personType = type;

        setPersons((prev) => [...prev, newPerson]);
      } else {
        const updatedPerson = {
          id: editPersonId,
          ...formData,
        };

        const response = await fetch(
          `${apiBaseUrl}/api/v1/persons/${editPersonId}`,
          {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updatedPerson),
          }
        );

        if (response.status !== 204) {
          const errorText = await response.text();
          alert(`Failed to update person: ${response.status} - ${errorText}`);
          return;
        }

        setPersons((prev) =>
          prev.map((p) =>
            p.id === editPersonId ? { ...p, ...formData, personType: type } : p
          )
        );
      }

      setShowModal(false);
    } catch (error) {
      console.error("Error saving person:", error);
      alert("An error occurred while saving the person.");
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
