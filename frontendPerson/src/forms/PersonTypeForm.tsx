import React, { useState, useEffect } from "react";

interface Props {
  initialValue: string;
  onSubmit: (description: string) => void;
  submitting: boolean;
}

function PersonTypeForm({ initialValue, onSubmit, submitting }: Props) {
  const [description, setDescription] = useState(initialValue);
  const [error, setError] = useState("");

  useEffect(() => {
    setDescription(initialValue);
    setError("");
  }, [initialValue]);

  const validate = (): boolean => {
    if (!description.trim()) {
      setError("Description is required");
      return false;
    }
    if (description.length < 2 && description.length > 30) {
      setError("Description must be between 2 and 30 characters");
      return false;
    }
    const validPattern = /^[a-zA-Z\s]+$/;
    if (!validPattern.test(description)) {
      setError("Description can only contain letters and spaces");
      return false;
    }
    return true;
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (validate()) {
      onSubmit(description);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="mb-3">
        <label className="form-label">Description</label>
        <input
          type="text"
          className={`form-control ${error ? "is-invalid" : ""}`}
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          disabled={submitting}
        />
        {error && <div className="invalid-feedback">{error}</div>}
      </div>
      <div className="text-end">
        <button type="submit" className="btn btn-primary" disabled={submitting}>
          {submitting ? "Saving..." : "Save"}
        </button>
      </div>
    </form>
  );
}

export default PersonTypeForm;
