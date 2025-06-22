import PersonTypeForm from './PersonTypeForm';

interface Props {
  show: boolean;
  onClose: () => void;
  onSubmit: (description: string) => void;
  submitting: boolean;
  initialValue: string;
  error?: string | null;
}

function PersonTypeModal({ show, onClose, onSubmit, submitting, initialValue, error }: Props) {
  if (!show) return null;

  return (
    <>
      <div className="modal fade show d-block" tabIndex={-1}>
        <div className="modal-dialog">
          <div className="modal-content">

            <div className="modal-header">
              <h5 className="modal-title">Create Person Type</h5>
              <button type="button" className="btn-close" onClick={onClose} disabled={submitting} />
            </div>

            <div className="modal-body">
              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}
              <PersonTypeForm
                initialValue={initialValue}
                onSubmit={onSubmit}
                submitting={submitting}
              />
            </div>

            <div className="modal-footer">
              <button className="btn btn-secondary" onClick={onClose} disabled={submitting}>
                Cancel
              </button>
            </div>

          </div>
        </div>
      </div>
      <div className="modal-backdrop fade show"></div>
    </>
  );
}

export default PersonTypeModal;
