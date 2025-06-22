import { useState } from "react";
import type { PersonType } from "../models/model";
import PersonTypeModal from "./PersonTypeModal";

type Props = {
  personTypes: PersonType[];
  onPersonTypeAdded: () => void;
};

function ListPersonTypes({ personTypes, onPersonTypeAdded }: Props) {
  const [showModal, setShowModal] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

  const handleCreate = async (description: string) => {
    try {
      setSubmitting(true);
      setError(null);

      const response = await fetch(`${apiBaseUrl}/api/persontypes`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ description }),
      });

      if (!response.ok) {
        const message = await response.text();
        throw new Error(message || `Failed to create: ${response.statusText}`);
      }

      await response.json();
      await onPersonTypeAdded(); // Refresh from server
      setShowModal(false);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unknown error");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="container mt-5">
      <div className="d-flex justify-content-between align-items-center mb-2">
        <h2 className="mb-0">List of Person Types</h2>
        <button
          className="btn btn-success"
          onClick={() => {
            setShowModal(true);
            setError(null);
          }}
          title="Add a person type"
        >
          <i className="bi bi-plus-lg"></i>
        </button>
      </div>

      {personTypes.length === 0 ? (
        <p>No person types found.</p>
      ) : (
        <ul className="list-group">
          {personTypes.map((type) => (
            <li key={type.id} className="list-group-item">
              <strong>{type.description}</strong> (ID: {type.id})
            </li>
          ))}
        </ul>
      )}

      <PersonTypeModal
        show={showModal}
        onClose={() => {
          setShowModal(false);
          setError(null);
        }}
        onSubmit={handleCreate}
        submitting={submitting}
        initialValue=""
        error={error}
      />
    </div>
  );
}

export default ListPersonTypes;
