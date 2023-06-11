package com.pauloreis.catalog.admin.domain.category;

import java.util.Optional;

import com.pauloreis.catalog.admin.domain.pagination.Pagination;

public interface CategoryGateway {

  Category create(Category aCategory);

  void deleteById(CategoryID anId);

  Optional<Category> findById(CategoryID anId);

  Category update(Category aCategory);

  Pagination<Category> findAll(CategorySearchQuery aQuery);
}
