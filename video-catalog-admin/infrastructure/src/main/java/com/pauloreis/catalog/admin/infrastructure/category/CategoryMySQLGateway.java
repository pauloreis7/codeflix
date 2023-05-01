package com.pauloreis.catalog.admin.infrastructure.category;

import java.util.Optional;

import org.springframework.stereotype.Service;

import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.category.CategoryID;
import com.pauloreis.catalog.admin.domain.category.CategorySearchQuery;
import com.pauloreis.catalog.admin.domain.pagination.Pagination;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryJpaEntity;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryRepository;

@Service
public class CategoryMySQLGateway implements CategoryGateway {

  private final CategoryRepository repository;

  public CategoryMySQLGateway(final CategoryRepository repository) {
    this.repository = repository;
  }

  @Override
  public Category create(final Category aCategory) {
    return repository.save(CategoryJpaEntity.from(aCategory)).toAggregate();
  }

  @Override
  public void deleteById(final CategoryID anId) {
    // TODO Auto-generated method stub

  }

  @Override
  public Pagination<Category> findAll(final CategorySearchQuery aQuery) {
    // TODO Auto-generated method stub
    return null;
  }

  @Override
  public Optional<Category> findById(final CategoryID anId) {
    // TODO Auto-generated method stub
    return Optional.empty();
  }

  @Override
  public Category update(final Category aCategory) {
    // TODO Auto-generated method stub
    return null;
  }

}
