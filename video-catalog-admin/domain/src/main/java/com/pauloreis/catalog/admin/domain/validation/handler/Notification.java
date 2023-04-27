package com.pauloreis.catalog.admin.domain.validation.handler;

import java.util.ArrayList;
import java.util.List;

import com.pauloreis.catalog.admin.domain.exceptions.DomainException;
import com.pauloreis.catalog.admin.domain.validation.Error;
import com.pauloreis.catalog.admin.domain.validation.ValidationHandler;

public class Notification implements ValidationHandler {
  private final List<Error> errors;

  private Notification(final List<Error> errors) {
    this.errors = errors;
  }

  public static Notification create() {
    return new Notification(new ArrayList<>());
  }

  public static Notification create(final Error anError) {
    return new Notification(new ArrayList<>()).append(anError);
  }

  @Override
  public Notification append(final Error anError) {
    errors.add(anError);
    return this;
  }

  @Override
  public Notification append(final ValidationHandler aHandler) {
    errors.addAll(aHandler.getErrors());
    return this;
  }

  @Override
  public Notification validate(final Validation aValidation) {
    try {
      this.validate(aValidation);
    } catch (final DomainException exception) {
      errors.addAll(exception.getErrors());
    } catch (final Throwable exception) {
      errors.add(new Error(exception.getMessage()));
    }

    return this;
  }

  @Override
  public List<Error> getErrors() {
    return errors;
  }
}
