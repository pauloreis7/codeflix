package com.pauloreis.catalog.admin.application.category.retrieve.get;

import java.util.Objects;
import java.util.function.Supplier;

import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.category.CategoryID;
import com.pauloreis.catalog.admin.domain.exceptions.DomainException;
import com.pauloreis.catalog.admin.domain.validation.Error;

public class DefaultGetCategoryByIdUseCase extends GetCategoryByIdUseCase {

  private final CategoryGateway categoryGateway;

  DefaultGetCategoryByIdUseCase(final CategoryGateway categoryGateway) {
    this.categoryGateway = Objects.requireNonNull(categoryGateway);
  }

  @Override
  public CategoryOutput execute(final String anIn) {
    final var anCategoryID = CategoryID.from(anIn);

    return categoryGateway.findById(anCategoryID)
        .map(CategoryOutput::from)
        .orElseThrow(notFound(anCategoryID));
  }

  private Supplier<DomainException> notFound(final CategoryID anId) {
    final var anErrorMessage = String.format("Category with ID %s was not found", anId.getValue());

    return () -> DomainException.with(new Error(anErrorMessage));
  }
}
