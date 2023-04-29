package com.pauloreis.catalog.admin.application.category.delete;

import java.util.Objects;

import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.category.CategoryID;

public class DefaultDeleteCategoryUseCase extends DeleteCategoryUseCase {

  private final CategoryGateway categoryGateway;

  DefaultDeleteCategoryUseCase(final CategoryGateway categoryGateway) {
    this.categoryGateway = Objects.requireNonNull(categoryGateway);
  }

  @Override
  public void execute(final String anIn) {
    categoryGateway.deleteById(CategoryID.from(anIn));
  }
}
