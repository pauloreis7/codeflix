package com.pauloreis.catalog.admin.infrastructure.configuration.usecases;

import com.pauloreis.catalog.admin.application.category.create.CreateCategoryUseCase;
import com.pauloreis.catalog.admin.application.category.create.DefaultCreateCategoryUseCase;
import com.pauloreis.catalog.admin.application.category.delete.DefaultDeleteCategoryUseCase;
import com.pauloreis.catalog.admin.application.category.delete.DeleteCategoryUseCase;
import com.pauloreis.catalog.admin.application.category.retrieve.get.DefaultGetCategoryByIdUseCase;
import com.pauloreis.catalog.admin.application.category.retrieve.get.GetCategoryByIdUseCase;
import com.pauloreis.catalog.admin.application.category.retrieve.list.DefaultListCategoriesUseCase;
import com.pauloreis.catalog.admin.application.category.retrieve.list.ListCategoriesUseCase;
import com.pauloreis.catalog.admin.application.category.update.DefaultUpdateCategoryUseCase;
import com.pauloreis.catalog.admin.application.category.update.UpdateCategoryUseCase;
import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class CategoryUseCaseConfig {

  private final CategoryGateway categoryGateway;

  public CategoryUseCaseConfig(final CategoryGateway categoryGateway) {
    this.categoryGateway = categoryGateway;
  }

  @Bean
  public CreateCategoryUseCase createCategoryUseCase() {
    return new DefaultCreateCategoryUseCase(categoryGateway);
  }

  @Bean
  public UpdateCategoryUseCase updateCategoryUseCase() {
    return new DefaultUpdateCategoryUseCase(categoryGateway);
  }

  @Bean
  public GetCategoryByIdUseCase getCategoryByIdUseCase() {
    return new DefaultGetCategoryByIdUseCase(categoryGateway);
  }

  @Bean
  public ListCategoriesUseCase listCategoriesUseCase() {
    return new DefaultListCategoriesUseCase(categoryGateway);
  }

  @Bean
  public DeleteCategoryUseCase deleteCategoryUseCase() {
    return new DefaultDeleteCategoryUseCase(categoryGateway);
  }
}