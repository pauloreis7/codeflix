package com.pauloreis.catalog.admin.application.category.retrieve.list;

import java.util.Objects;

import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.category.CategorySearchQuery;
import com.pauloreis.catalog.admin.domain.pagination.Pagination;

public class DefaultListCategoriesUseCase extends ListCategoriesUseCase {

  private CategoryGateway categoryGateway;

  public DefaultListCategoriesUseCase(final CategoryGateway categoryGateway) {
    this.categoryGateway = Objects.requireNonNull(categoryGateway);
  }

  @Override
  public Pagination<CategoryListOutput> execute(final CategorySearchQuery aQuery) {
    return categoryGateway.findAll(aQuery).map(CategoryListOutput::from);
  }
}
