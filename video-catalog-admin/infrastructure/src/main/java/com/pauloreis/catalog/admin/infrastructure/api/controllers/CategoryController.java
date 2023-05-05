package com.pauloreis.catalog.admin.infrastructure.api.controllers;

import java.util.Objects;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RestController;

import com.pauloreis.catalog.admin.application.category.create.CreateCategoryUseCase;
import com.pauloreis.catalog.admin.domain.pagination.Pagination;
import com.pauloreis.catalog.admin.infrastructure.api.CategoryAPI;

@RestController
public class CategoryController implements CategoryAPI {

  private final CreateCategoryUseCase createCategoryUseCase;

  public CategoryController(final CreateCategoryUseCase createCategoryUseCase) {
    this.createCategoryUseCase = Objects.requireNonNull(createCategoryUseCase);
  }

  @Override
  public ResponseEntity<?> createCategory() {
    // TODO Auto-generated method stub
    return null;
  }

  @Override
  public Pagination<?> listCategories(
      final String search,
      final int page,
      final int perPage,
      final String sort,
      final String direction) {
    // TODO Auto-generated method stub
    return null;
  }

}
