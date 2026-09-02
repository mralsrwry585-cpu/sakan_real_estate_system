/**
 * SAKAN — Owner/Lessor dashboard interaction layer.
 * Modal management, sidebar, DataTables RTL, wizard chips, media grid, upload zone.
 */
(function () {
  'use strict';

  // ------------------------------------------------------------------
  // Sidebar (mobile) toggle
  // ------------------------------------------------------------------
  function initSidebar() {
    var sidebar = document.querySelector('[data-sidebar]');
    var toggle = document.querySelector('[data-sidebar-toggle]');
    var backdrop = document.querySelector('[data-sidebar-backdrop]');

    function open() {
      if (sidebar) sidebar.classList.add('open');
      if (backdrop) backdrop.classList.add('show');
    }
    function close() {
      if (sidebar) sidebar.classList.remove('open');
      if (backdrop) backdrop.classList.remove('show');
    }

    if (toggle) toggle.addEventListener('click', function () {
      if (sidebar && sidebar.classList.contains('open')) close(); else open();
    });
    if (backdrop) backdrop.addEventListener('click', close);
  }

  // ------------------------------------------------------------------
  // Custom modal engine (data-modal / data-open-modal / data-modal-close)
  // ------------------------------------------------------------------
  function initModals() {
    document.addEventListener('click', function (e) {
      var opener = e.target.closest('[data-open-modal]');
      if (opener) {
        var id = opener.getAttribute('data-open-modal');
        var modal = document.getElementById(id);
        if (modal) {
          modal.classList.add('show');
          document.body.classList.add('sakan-modal-open');
        }
        return;
      }

      var closer = e.target.closest('[data-modal-close]');
      if (closer) {
        var modalEl = closer.closest('[data-modal]');
        if (modalEl) hideModal(modalEl);
        return;
      }
    });

    // Backdrop click to close
    document.querySelectorAll('[data-modal]').forEach(function (m) {
      m.addEventListener('mousedown', function (e) {
        if (e.target === m) hideModal(m);
      });
    });

    // ESC key
    document.addEventListener('keydown', function (e) {
      if (e.key === 'Escape') {
        document.querySelectorAll('[data-modal].show').forEach(hideModal);
      }
    });
  }

  function hideModal(modal) {
    modal.classList.remove('show');
    if (!document.querySelector('[data-modal].show')) {
      document.body.classList.remove('sakan-modal-open');
    }
  }

  // ------------------------------------------------------------------
  // Wizard type/contract chips → hidden input
  // ------------------------------------------------------------------
function initWizardChips() {
    document.querySelectorAll('[data-type-chip]').forEach(function (chip) {
      var isGold = chip.classList.contains('active-gold');
      chip.dataset.tone = isGold ? 'gold' : 'primary';
      chip.addEventListener('click', function () {
        var section = chip.closest('.row, .card-sakan, form');
        var hidden = section ? section.querySelector('input[type="hidden"]') : null;
        var chips = section ? section.querySelectorAll('[data-type-chip]') : [];
        chips.forEach(function (c) {
          c.classList.remove('active-primary', 'active-gold');
        });
        chip.classList.add(chip.dataset.tone === 'gold' ? 'active-gold' : 'active-primary');
        if (hidden) hidden.value = chip.dataset.value;
      });
    });
  }

  // ------------------------------------------------------------------
  // Amenity checkbox pills (step 3)
  // ------------------------------------------------------------------
  function initAmenityPills() {
    document.querySelectorAll('.filter-pill input[type="checkbox"]').forEach(function (input) {
      input.addEventListener('change', function () {
        var label = input.closest('.filter-pill');
        if (label) label.classList.toggle('active', input.checked);
      });
    });
  }

  // ------------------------------------------------------------------
  // Viewing accept modal — time-slot picker
  // ------------------------------------------------------------------
  function initTimeSlots() {
    document.querySelectorAll('[data-time-slot]').forEach(function (slot) {
      slot.addEventListener('click', function () {
        var modal = slot.closest('[data-modal]');
        var hidden = modal ? modal.querySelector('#RequestedTime') : null;
        var row = slot.closest('.row');
        if (row) row.querySelectorAll('[data-time-slot]').forEach(function (s) {
          s.classList.remove('active-primary');
        });
        slot.classList.add('active-primary');
        if (hidden) hidden.value = slot.textContent.trim();
      });
    });
  }

  // ------------------------------------------------------------------
  // Media grid — reorder (light) + cover/delete
  // ------------------------------------------------------------------
  function initMediaGrid() {
    var grid = document.querySelector('[data-media-grid]');
    if (!grid) return;

    var propertyId = grid.getAttribute('data-property-id');
    var items = grid.querySelectorAll('[data-media-item]');

    if (!window.Sortable || items.length < 2) {
      // If Sortable isn't loaded, keep grid static (still usable via buttons).
      return;
    }

    window.Sortable.create(grid, {
      animation: 200,
      direction: 'rtl',
      onEnd: function () {
        var order = Array.prototype.map.call(
          grid.querySelectorAll('[data-media-item]'),
          function (el, idx) {
            return { mediaId: el.getAttribute('data-media-item'), displayOrder: idx + 1 };
          }
        );

        fetch('/owner/properties/' + propertyId + '/media/reorder', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getAntiForgeryToken()
          },
          body: JSON.stringify({ items: order })
        })
          .then(function (r) { return r.json(); })
          .then(function (res) {
            if (!res.ok) {
              showToast('danger', 'تعذر حفظ الترتيب الجديد.');
            }
          })
          .catch(function () {
            showToast('danger', 'تعذر حفظ الترتيب الجديد.');
          });
      }
    });
  }

  // ------------------------------------------------------------------
  // Upload zone visual feedback (drag-over)
  // ------------------------------------------------------------------
  function initUploadZone() {
    document.querySelectorAll('[data-upload-zone]').forEach(function (zone) {
      ['dragenter', 'dragover'].forEach(function (ev) {
        zone.addEventListener(ev, function (e) {
          e.preventDefault();
          zone.classList.add('dragover');
        });
      });
      ['dragleave', 'drop'].forEach(function (ev) {
        zone.addEventListener(ev, function (e) {
          e.preventDefault();
          zone.classList.remove('dragover');
        });
      });
    });
  }

  // ------------------------------------------------------------------
  // DataTables RTL/Arabic init helper
  // ------------------------------------------------------------------
  function initDataTables() {
    document.querySelectorAll('table[data-datatable]').forEach(function (table) {
      if (!window.jQuery || !window.jQuery.fn || !window.jQuery.fn.DataTable) return;
      window.jQuery(table).DataTable({
        language: { url: 'https://cdn.datatables.net/plug-ins/1.13.8/i18n/ar.json' },
        dom: '<"d-flex justify-content-between align-items-center mb-3"lf>rt<"d-flex justify-content-between align-items-center mt-3"ip>',
        pageLength: 10,
        responsive: true,
        order: []
      });
    });
  }

  // ------------------------------------------------------------------
  // Toasts
  // ------------------------------------------------------------------
  function showToast(type, message) {
    var container = document.querySelector('[data-toast-container]');
    if (!container) {
      container = document.createElement('div');
      container.setAttribute('data-toast-container', '');
      container.style.cssText = 'position:fixed;bottom:1.25rem;left:1.25rem;z-index:1200;display:flex;flex-direction:column;gap:0.5rem;';
      document.body.appendChild(container);
    }
    var toast = document.createElement('div');
    toast.className = 'sakan-toast sakan-toast-' + type;
    toast.textContent = message;
    container.appendChild(toast);
    setTimeout(function () {
      toast.classList.add('show');
    }, 10);
    setTimeout(function () {
      toast.classList.remove('show');
      setTimeout(function () { toast.remove(); }, 300);
    }, 4000);
  }

  function getAntiForgeryToken() {
    // Token is emitted in a hidden input named "__RequestVerificationToken".
    var input = document.querySelector('input[name="__RequestVerificationToken"]');
    return input ? input.value : '';
  }

  // ------------------------------------------------------------------
  // Bootstrap
  // ------------------------------------------------------------------
  document.addEventListener('DOMContentLoaded', function () {
    initSidebar();
    initModals();
    initWizardChips();
    initAmenityPills();
    initTimeSlots();
    initMediaGrid();
    initUploadZone();
    initDataTables();
  });

  window.SakanUI = {
    showToast: showToast,
    closeModal: hideModal
  };
})();

