import React, { useState, useEffect } from 'react';
import type { PersonCreate, PersonType } from '..//models/model';

interface Props {
  initialForm: PersonCreate;
  personTypes: PersonType[];
  onSubmit: (data: PersonCreate) => void;
}

function PersonForm({ initialForm, personTypes, onSubmit }: Props) {
  const [form, setForm] = useState(initialForm);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  useEffect(() => {
    setForm(initialForm); // reset when editing a new person
    setErrors({});
  }, [initialForm]);

  const validate = (): boolean => {
    const errs: typeof errors = {};
    if (!form.name.trim()) errs.name = 'Name is required';
    else if (form.name.length > 50) errs.name = 'Name must be under 50 characters';

    if (!form.age) errs.age = 'Age is required';
    else if (form.age < 5 || form.age > 60) errs.age = 'Age must be between 5 and 60';

    if (!form.personTypeId) errs.personTypeId = 'Person type is required';

    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: name === 'age' || name === 'personTypeId' ? parseInt(value) : value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (validate()) {
      onSubmit(form);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="mb-3">
        <label className="form-label">Name</label>
        <input
          name="name"
          type="text"
          className={`form-control ${errors.name ? 'is-invalid' : ''}`}
          value={form.name}
          onChange={handleChange}
        />
        {errors.name && <div className="invalid-feedback">{errors.name}</div>}
      </div>

      <div className="mb-3">
        <label className="form-label">Age</label>
        <input
          name="age"
          type="number"
          className={`form-control ${errors.age ? 'is-invalid' : ''}`}
          value={form.age}
          onChange={handleChange}
        />
        {errors.age && <div className="invalid-feedback">{errors.age}</div>}
      </div>

      <div className="mb-3">
        <label className="form-label">Person Type</label>
        <select
          name="personTypeId"
          className={`form-select ${errors.personTypeId ? 'is-invalid' : ''}`}
          value={form.personTypeId}
          onChange={handleChange}
        >
          {personTypes.map(pt => (
            <option key={pt.id} value={pt.id}>
              {pt.description}
            </option>
          ))}
        </select>
        {errors.personTypeId && <div className="invalid-feedback">{errors.personTypeId}</div>}
      </div>

      <div className="text-end">
        <button type="submit" className="btn btn-primary">
          Save
        </button>
      </div>
    </form>
  );
}

export default PersonForm;
