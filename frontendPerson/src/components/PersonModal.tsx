import type { PersonCreate, PersonType } from '../models/model';
import PersonForm from './PersonForm';

interface Props {
  show: boolean;
  onClose: () => void;
  onSave: (data: PersonCreate) => void;
  form: PersonCreate;
  personTypes: PersonType[];
  isEdit: boolean;
}

function PersonModal({ show, onClose, onSave, form, personTypes, isEdit }: Props) {
  return (
    <div className="modal show d-block" tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{isEdit ? 'Edit Person' : 'Add Person'}</h5>
            <button type="button" className="btn-close" onClick={onClose}></button>
          </div>
          <div className="modal-body">
            <PersonForm initialForm={form} personTypes={personTypes} onSubmit={onSave} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default PersonModal;
