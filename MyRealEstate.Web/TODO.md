# Owner/Lessor Module Completion — TODO

## Stage F — High-Fidelity Razor Views
- [x] F0. Fix build: add `using MyRealEstate.Web.Helpers` to RequestsViewModel + PropertyDetailsViewModel; fix ViewingRequestDto.Number
- [x] F1. PropertyWizardController — load real amenity catalog into AmenityGroups (LoadAmenityCatalogAsync)
- [ ] F2. `Views/Lessor/Properties/Index.cshtml` — high-fidelity table (thumbnails, badges, actions, filter pills, pagination, DataTables)
- [ ] F3. `Views/Lessor/Dashboard/Index.cshtml` — stat cards partial + real trend chart + status distribution + recent requests
- [ ] F4. Wizard views — 5 steps (basic/location/amenities/media/review) via `PropertyWizard/Index.cshtml`
- [ ] F5. `Views/Lessor/PropertyDetails/Index.cshtml` — gallery, description, specs, amenities, status change, address
- [ ] F6. `Views/Lessor/Media/Index.cshtml` — upload zone + gallery grid + cover/delete/reorder
- [ ] F7. `Views/Lessor/ViewingRequests/Index.cshtml` — stats + filters + DataTables + accept/reject dialogs
- [ ] F8. `Views/Lessor/BookingRequests/Index.cshtml` — stats + filters + DataTables + approve/reject dialogs

## Stage G — JavaScript behaviors (site.js)
- [ ] G1. Sidebar toggle (mobile), sidebar backdrop
- [ ] G2. Modal helpers (data-modal open/close/backdrop click)
- [ ] G3. Time-slot picker for viewing accept
- [ ] G4. DataTables Arabic/RTL init shared helper
- [ ] G5. Wizard client-side validations + chip selection
- [ ] G6. Media delete/cover/reorder interaction helpers

## Stage H — CSS polish (theme.css)
- [ ] H1. Media grid, property cards, sidebar badges, review groups, upload progress, publish readiness, skeletons

## Stage J — Verification
- [ ] J1. `dotnet restore` green
- [ ] J2. `dotnet build` green (0 errors, 0 warnings)
- [ ] J3. Verify routes / controllers / partials / viewmodels
- [ ] J4. Verify responsive + RTL
- [ ] J5. Smoke test against live API

