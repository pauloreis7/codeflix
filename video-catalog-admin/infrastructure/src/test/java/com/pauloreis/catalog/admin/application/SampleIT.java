package com.pauloreis.catalog.admin.application;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;

import com.pauloreis.catalog.admin.IntegrationTest;
import com.pauloreis.catalog.admin.application.category.create.CreateCategoryUseCase;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryRepository;

@IntegrationTest
public class SampleIT {

  @Autowired
  private CreateCategoryUseCase useCase;

  @Autowired
  private CategoryRepository categoryRepository;

  @Test
  public void testInjects() {
    Assertions.assertNotNull(useCase);
    Assertions.assertNotNull(categoryRepository);
  }
}